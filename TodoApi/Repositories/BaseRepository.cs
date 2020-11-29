using System;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly TodoContext Context;

        protected BaseRepository(TodoContext context)
        {
            Context = context;
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await Context.Set<T>().FindAsync(id);
        }
    }
}