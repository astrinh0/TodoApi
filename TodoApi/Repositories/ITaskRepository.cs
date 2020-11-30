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

        Task<Tasks> ChangeTasksAsync(long id, long todo, Tasks tasks);



    }
}
