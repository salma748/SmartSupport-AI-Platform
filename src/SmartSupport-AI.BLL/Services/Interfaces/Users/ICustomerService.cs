using SmartSupport_AI.BLL.DTOs.Users;

namespace SmartSupport_AI.BLL.Services.Interfaces.Users
{
    public interface ICustomerService
    {
        Task<CustomerProfileDto?> GetByUserIdAsync(int userId);
    }
}
