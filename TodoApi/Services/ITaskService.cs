using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITaskService
    {
        Task<Tasks> GetTasksByIdAsync(long userId, long todoId);

        Task<Tasks> CreateTaskAsync(Tasks task);
    }
}
