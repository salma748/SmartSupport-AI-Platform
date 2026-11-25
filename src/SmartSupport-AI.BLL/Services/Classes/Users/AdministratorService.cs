using SmartSupport_AI.BLL.DTOs.Users;
using SmartSupport_AI.BLL.Services.Interfaces.Users;
using SmartSupport_AI.DAL.Repositories.Interfaces.Users;

namespace SmartSupport_AI.BLL.Services.Classes.Users
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IUserRepository _userRepository;

        public AdministratorService(IAdministratorRepository administratorRepository, IUserRepository userRepository)
        {
            _administratorRepository = administratorRepository;
            _userRepository = userRepository;
        }

        public async Task<AdministratorProfileDto?> GetByIdAsync(int adminId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(adminId);

            var administrator = await _administratorRepository.GetByIdAsync(adminId);
            if (administrator is null)
            {
                return null;
            }

            var user = await _userRepository.GetByIdAsync(administrator.UserId);
            if (user is null)
            {
                return null;
            }

            return new AdministratorProfileDto
            {
                AdminId = administrator.AdminId,
                UserId = administrator.UserId,
                FullName = user.FullName
            };
        }
    }
}
