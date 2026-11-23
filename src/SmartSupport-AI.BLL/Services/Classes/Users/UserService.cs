using SmartSupport_AI.BLL.DTOs.Users;
using SmartSupport_AI.BLL.Services.Interfaces.Users;
using SmartSupport_AI.DAL.Repositories.Interfaces.Users;

namespace SmartSupport_AI.BLL.Services.Classes.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfileDto?> GetProfileAsync(int userId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return null;
            }

            return new UserProfileDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone
            };
        }
    }
}
