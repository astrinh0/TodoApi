using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<Tasks> GetTasksByIdAsync(long userId, long todoId)
        {
            return await _taskRepository.GetTasksByIdAsync(userId, todoId);
        }

        public async Task<Tasks> CreateTaskAsync(Tasks task)
        {
            return await _taskRepository.CreateTaskAsync(task);
        }
    }
}
