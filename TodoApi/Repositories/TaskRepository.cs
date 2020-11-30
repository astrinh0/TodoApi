using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Filters;
using TodoApi.Models;
using TodoApi.Wrappers;

namespace TodoApi.Repositories
{
    public class TaskRepository : BaseRepository<Tasks>, ITaskRepository
    {
        
        public TaskRepository(TodoContext context) : base(context)
        {

        }

        public async Task<Tasks> GetTasksByIdAsync(long userId, long todoId)
        {
            return await Context.Set<Tasks>().FindAsync(userId, todoId);
        }

        public async Task<Tasks> CreateTaskAsync(Tasks task)
        {
            Context.Tasks.Add(task);
            task.User = UserExists(task.UserId);
            task.Todo = TodoExists(task.TodoId);
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TasksExists(task.UserId, task.TodoId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return task;
        }

        public async Task<IEnumerable<Tasks>> GetAllTasksAsync([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await Context.Tasks
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();
            return new PagedResponse<List<Tasks>>(
                pagedData, validFilter.PageNumber, validFilter.PageSize);
                //return Ok(new PagedResponse<List<Tasks>>(pagedData, validFilter.PageNumber, validFilter.PageSize));

        }

        public bool TasksExists(long id, long todoid)
        {
            return Context.Tasks.Any(e => e.UserId == id & e.TodoId == todoid);
        }

        public User UserExists(long id)
        {
            User aux;
            aux = Context.User.Find(id);

            return aux;

        }

        public Todo TodoExists(long id)
        {
            Todo aux;
            aux = Context.Todo.Find(id);

            return aux;

        }
    }
}
