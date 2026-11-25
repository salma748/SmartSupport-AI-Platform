using SmartSupport_AI.BLL.DTOs.KnowledgeBase;

namespace SmartSupport_AI.BLL.Services.Interfaces.KnowledgeBase
{
    public interface ISystemPolicyService
    {
        Task<SystemPolicyDto?> GetByIdAsync(int policyId);

        Task<SystemPolicyDto?> GetByPolicyNameAsync(string policyName);
    }
}
