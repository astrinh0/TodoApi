using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id);
        Task<bool> ExistsAsync(long id);
        IQueryable<T> GetPagedAsync(int skip = 0, int pageSize = 10);
    }
}