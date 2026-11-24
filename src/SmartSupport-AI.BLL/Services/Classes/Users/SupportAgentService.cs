using SmartSupport_AI.BLL.DTOs.Users;
using SmartSupport_AI.BLL.Services.Interfaces.Users;
using SmartSupport_AI.DAL.Repositories.Interfaces.Users;

namespace SmartSupport_AI.BLL.Services.Classes.Users
{
    public class SupportAgentService : ISupportAgentService
    {
        private readonly ISupportAgentRepository _supportAgentRepository;
        private readonly IUserRepository _userRepository;

        public SupportAgentService(ISupportAgentRepository supportAgentRepository, IUserRepository userRepository)
        {
            _supportAgentRepository = supportAgentRepository;
            _userRepository = userRepository;
        }

        public async Task<SupportAgentProfileDto?> GetByUserIdAsync(int userId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);

            var supportAgent = await _supportAgentRepository.GetByUserIdAsync(userId);
            if (supportAgent is null)
            {
                return null;
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return null;
            }

            return new SupportAgentProfileDto
            {
                AgentId = supportAgent.AgentId,
                UserId = supportAgent.UserId,
                FullName = user.FullName,
                Department = supportAgent.Department,
                SkillLevel = supportAgent.SkillLevel,
                Availability = supportAgent.Availability
            };
        }
    }
}
