using System.Threading.Tasks;

namespace TodoApi.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id);
    }
}