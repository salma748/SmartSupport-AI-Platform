using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SmartSupport_AI.BLL;

public sealed class AIProviderOptions
{
    public string Model { get; init; } = "gemini-3.7-flash";
    public int MaxOutputTokens { get; init; } = 1024;
    public bool AllowCustomerContent { get; init; }
}

public interface IAIProvider
{
    Task<JsonElement?> GenerateAsync(string instruction, IReadOnlyList<object> turns, object? tools, CancellationToken ct);
}

public sealed class GeminiAIProvider(HttpClient client, AIProviderOptions options, string? apiKey) : IAIProvider
{
    public async Task<JsonElement?> GenerateAsync(string instruction, IReadOnlyList<object> turns, object? tools, CancellationToken ct)
    {
        if (!options.AllowCustomerContent || string.IsNullOrWhiteSpace(apiKey)) return null;
        if (!Regex.IsMatch(options.Model, @"\Agemini-[a-z0-9.-]{1,80}\z")) return null;

        using var deadline = CancellationTokenSource.CreateLinkedTokenSource(ct);
        deadline.CancelAfter(TimeSpan.FromSeconds(35));
        for (var attempt = 0; attempt < 3; attempt++)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post,
                $"https://generativelanguage.googleapis.com/v1beta/models/{options.Model}:generateContent");
            request.Headers.Add("x-goog-api-key", apiKey);
            var body = new Dictionary<string, object>
            {
                ["systemInstruction"] = new { parts = new[] { new { text = instruction } } },
                ["contents"] = turns,
                ["generationConfig"] = new { temperature = 0.2, maxOutputTokens = Math.Clamp(options.MaxOutputTokens, 128, 4096) }
            };
            if (tools is not null) body["tools"] = tools;
            request.Content = JsonContent.Create(body);
            var delay = TimeSpan.FromMilliseconds(250 * (1 << attempt) + Random.Shared.Next(50, 200));
            try
            {
                using var response = await client.SendAsync(request, deadline.Token);
                if (response.IsSuccessStatusCode)
                {
                    using var payload = await response.Content.ReadFromJsonAsync<JsonDocument>(cancellationToken: deadline.Token);
                    if (payload is null || payload.RootElement.ValueKind != JsonValueKind.Object || !payload.RootElement.TryGetProperty("candidates", out var candidates) ||
                        candidates.ValueKind != JsonValueKind.Array || candidates.GetArrayLength() == 0 ||
                        candidates[0].ValueKind != JsonValueKind.Object || !candidates[0].TryGetProperty("content", out var content) ||
                        content.ValueKind != JsonValueKind.Object) return null;
                    return content.Clone();
                }
                if ((int)response.StatusCode is not (408 or 429 or 500 or 502 or 503 or 504)) return null;
                var retryAfter = response.Headers.RetryAfter;
                var retryDelay = retryAfter?.Delta ?? retryAfter?.Date - DateTimeOffset.UtcNow;
                if (retryDelay > delay) delay = retryDelay.Value;
                if (delay > TimeSpan.FromSeconds(3)) return null;
            }
            catch (HttpRequestException) { }
            catch (JsonException) { return null; }
            catch (OperationCanceledException) when (!ct.IsCancellationRequested) { return null; }

            if (attempt == 2) break;
            try { await Task.Delay(delay, deadline.Token); }
            catch (OperationCanceledException) when (!ct.IsCancellationRequested) { return null; }
        }
        return null;
    }
}
