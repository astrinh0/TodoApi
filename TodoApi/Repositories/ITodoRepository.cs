using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository : IBaseRepository<Todo>
    {
    }
}