using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Filters;
using TodoApi.Models;
using TodoApi.Wrappers;

namespace TodoApi.Services
{
    public interface ITaskService
    {
        Task<Tasks> GetTasksByIdAsync(long userId, long todoId);

        Task<Tasks> CreateTaskAsync(Tasks task);

        Task<PagedResponse<IList<Tasks>>> GetAllTasksAsync(PaginationFilter filter);

        Task<Tasks> ChangeTasksAsync(long id, long todo, Tasks tasks);

    }
}
