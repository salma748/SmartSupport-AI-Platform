using SmartSupport_AI.BLL.DTOs.Users;

namespace SmartSupport_AI.BLL.Services.Interfaces.Users
{
    public interface IAdministratorService
    {
        Task<AdministratorProfileDto?> GetByIdAsync(int adminId);
    }
}
