using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : BaseRepository<Todo>, ITodoRepository 
    {
        public TodoRepository(TodoContext context) : base(context)
        {
        }

    }
}