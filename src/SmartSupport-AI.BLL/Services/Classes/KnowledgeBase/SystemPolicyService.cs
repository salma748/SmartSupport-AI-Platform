using SmartSupport_AI.BLL.DTOs.KnowledgeBase;
using SmartSupport_AI.BLL.Services.Interfaces.KnowledgeBase;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.BLL.Services.Classes.KnowledgeBase
{
    public class SystemPolicyService : ISystemPolicyService
    {
        private const int MaxPolicyNameLength = 256;

        private readonly ISystemPolicyRepository _policyRepository;

        public SystemPolicyService(ISystemPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        public async Task<SystemPolicyDto?> GetByIdAsync(int policyId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(policyId);

            var policy = await _policyRepository.GetByIdAsync(policyId);
            return policy is null ? null : MapToDto(policy);
        }

        public async Task<SystemPolicyDto?> GetByPolicyNameAsync(string policyName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(policyName);
            if (policyName.Length > MaxPolicyNameLength)
            {
                throw new ArgumentException($"Policy name must be at most {MaxPolicyNameLength} characters.", nameof(policyName));
            }

            var policy = await _policyRepository.GetByPolicyNameAsync(policyName);
            return policy is null ? null : MapToDto(policy);
        }

        private static SystemPolicyDto MapToDto(SystemPolicy policy)
        {
            return new SystemPolicyDto
            {
                PolicyId = policy.PolicyId,
                PolicyName = policy.PolicyName,
                UpdatedAt = policy.UpdatedAt
            };
        }
    }
}
