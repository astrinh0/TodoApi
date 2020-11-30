using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService (IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> GetUserByIdAsync(long id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
    }
}


