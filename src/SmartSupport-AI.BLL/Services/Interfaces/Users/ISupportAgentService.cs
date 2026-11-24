using SmartSupport_AI.BLL.DTOs.Users;

namespace SmartSupport_AI.BLL.Services.Interfaces.Users
{
    public interface ISupportAgentService
    {
        Task<SupportAgentProfileDto?> GetByUserIdAsync(int userId);

        Task<SupportAgentProfileDto?> UpdateAvailabilityAsync(int agentId, string availability);
    }
}
