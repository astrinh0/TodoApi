using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Filters;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITaskRepository : IBaseRepository<Tasks>
    {
        Task<Tasks> GetTasksByIdAsync(long userId, long todoId);

        Task<Tasks> CreateTaskAsync(Tasks task);

        Task<IEnumerable<Tasks>> GetAllTasksAsync([FromQuery] PaginationFilter filter);

        bool TasksExists(long id, long todoid);

        User UserExists(long id);

        Todo TodoExists(long id);

    }
}
