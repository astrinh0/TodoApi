using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(long id);

    }
}
