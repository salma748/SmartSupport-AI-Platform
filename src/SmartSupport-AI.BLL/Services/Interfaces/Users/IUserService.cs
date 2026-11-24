using SmartSupport_AI.BLL.DTOs.Users;

namespace SmartSupport_AI.BLL.Services.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserProfileDto?> GetProfileAsync(int userId);

        Task<UserProfileDto?> UpdateProfileAsync(int userId, UpdateUserProfileDto dto);
    }
}
