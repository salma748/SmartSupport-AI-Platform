using SmartSupport_AI.BLL.DTOs.Users;
using SmartSupport_AI.BLL.Services.Interfaces.Users;
using SmartSupport_AI.DAL.Repositories.Interfaces.Users;

namespace SmartSupport_AI.BLL.Services.Classes.Users
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;

        public CustomerService(ICustomerRepository customerRepository, IUserRepository userRepository)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }

        public async Task<CustomerProfileDto?> GetByUserIdAsync(int userId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);

            var customer = await _customerRepository.GetByUserIdAsync(userId);
            if (customer is null)
            {
                return null;
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return null;
            }

            return new CustomerProfileDto
            {
                CustomerId = customer.CustomerId,
                UserId = customer.UserId,
                FullName = user.FullName,
                PreferredLanguage = customer.PreferredLanguage,
                RegisteredAt = customer.RegisteredAt
            };
        }
    }
}
