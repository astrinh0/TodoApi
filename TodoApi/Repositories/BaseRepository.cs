using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> ExistsAsync(long id)
        {
            return await Context.Set<T>().FindAsync(id) != null;
        }

        public IQueryable<T> GetPagedAsync(int skip = 0, int pageSize = 10)
        {
            return Context.Set<T>().Skip(skip).Take(pageSize);
        }
    }
}