using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Filters;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Wrappers;

namespace TodoApi.Repositories
{
    public class TaskRepository : BaseRepository<Tasks>, ITaskRepository
    {
        private readonly ITaskService _taskService;
      
        //Constructor
        public TaskRepository(TodoContext context, ITaskService taskService) : base(context)
        {
            _taskService = taskService;
        }

        // Find Tasks by 2 ids
        public async Task<Tasks> GetTasksByIdAsync(long userId, long todoId)
        {
            return await Context.Set<Tasks>().FindAsync(userId, todoId);
        }


        // Create a task association
        public async Task<Tasks> CreateTaskAsync(Tasks task)
        {
            Context.Tasks.Add(task);
            task.User = _taskService.UserExists(task.UserId);
            task.Todo = _taskService.TodoExists(task.TodoId);

            if (task.User == null | task.Todo == null)
            {
                return null;
            }
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

        public async Task<Tasks> ChangeTasksAsync(long id, long todo, Tasks tasks)
        {
            
            Context.Entry(tasks).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksExists(id, todo))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return tasks;
        }




        private bool TasksExists(long id, long todoid)
        {
            return Context.Tasks.Any(e => e.UserId == id & e.TodoId == todoid);
        }

    }
}
