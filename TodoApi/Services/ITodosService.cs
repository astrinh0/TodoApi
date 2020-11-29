using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodosService
    {
        Task<Todo> GetTodoByIdAsync(long id);
    }
}