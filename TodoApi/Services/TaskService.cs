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
        private readonly TodoContext _context;


        public TaskService(ITaskRepository taskRepository, TodoContext context)
        {
            _taskRepository = taskRepository;
            _context = context;
          
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

        public async Task<Tasks> ChangeTasksAsync(long id, long todo, Tasks tasks)
        {
            return await _taskRepository.ChangeTasksAsync(id, todo, tasks);
        }

        public User UserExists(long id)
        {
            User aux;
            aux = _context.User.Find(id);

            return aux;

        }

        public Todo TodoExists(long id)
        {
            Todo aux;
            aux = _context.Todo.Find(id);

            return aux;

        }

    }
}
