using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Filters;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Wrappers;

namespace TodoApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<PagedResponse<IList<Tasks>>> GetAllTasksAsync(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _taskRepository
                .GetPagedAsync((validFilter.PageNumber - 1) * validFilter.PageSize, validFilter.PageSize)
                .Take(validFilter.PageSize).ToListAsync();
            return new PagedResponse<IList<Tasks>>(
                pagedData, validFilter.PageNumber, validFilter.PageSize);
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
