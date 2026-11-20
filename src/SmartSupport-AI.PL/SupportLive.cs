using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SmartSupport_AI.BLL;

namespace SmartSupport_AI.PL;

public static class SupportPrincipal
{
    public static async Task<DeskIdentity?> ResolveAsync(HttpContext context, ClaimsPrincipal? principal = null)
    {
        if (!int.TryParse((principal ?? context.User).FindFirstValue(ClaimTypes.NameIdentifier), out var id)) return null;
        return await context.RequestServices.GetRequiredService<SupportDesk>().IdentityAsync(id, context.RequestAborted);
    }
}

public sealed class AgentPresence : ISupportPresence
{
    private readonly ConcurrentDictionary<string, int> connections = new();

    public void Connected(string connectionId, int agent) => connections[connectionId] = agent;
    public int? Disconnected(string connectionId) => connections.TryRemove(connectionId, out var agent) ? agent : null;
    public bool IsAvailable(int agentId) => connections.Values.Contains(agentId);
}

[Authorize]
public sealed class SupportHub(AgentPresence presence, SupportDesk desk, IHubContext<SupportHub> live) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var actor = await SupportPrincipal.ResolveAsync(Context.GetHttpContext()!);
        if (actor is null)
        {
            Context.Abort();
            return;
        }
        if (actor.Role == "Agent") presence.Connected(Context.ConnectionId, actor.ProfileId);
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{actor.UserId}");
        if (actor.Role == "Agent") await Groups.AddToGroupAsync(Context.ConnectionId, "agents");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (presence.Disconnected(Context.ConnectionId) is int agent && !presence.IsAvailable(agent))
        {
            using var deadline = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            var customers = await desk.RequeueDisconnectedAgentAsync(agent, deadline.Token);
            await live.Clients.Groups(customers.Select(id => $"user:{id}").Append("agents").ToArray())
                .SendAsync("Changed", cancellationToken: deadline.Token);
        }
        await base.OnDisconnectedAsync(exception);
    }
}
