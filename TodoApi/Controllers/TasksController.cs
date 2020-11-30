using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Filters;
using TodoApi.Models;
using TodoApi.Wrappers;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly ITaskService _tasksService;


        public TasksController(TodoContext context, ITaskService tasksService)
        {
            _context = context;
            _tasksService = tasksService;

        }

        /// <summary>
        /// Get all the tasks with pagination filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks([FromQuery] PaginationFilter filter)
        {
            return Ok(await _tasksService.GetAllTasksAsync(filter));
        }


        /// <summary>
        /// get a task by two id's
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todoId"></param>
        /// <returns></returns>
        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tasks>> GetTasks(long id, long todoId)
        {
            var tasks = await _tasksService.GetTasksByIdAsync(id, todoId);

            if (tasks == null)
            {
                return NotFound();
            }

            return tasks;
        }


        /// <summary>
        /// Change a task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todo"></param>
        /// <param name="tasks"></param>
        /// <returns></returns>
        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTasks(long id, long todo, Tasks tasks)
        {
            return Ok(await _tasksService.ChangeTasksAsync(id, todo, tasks));
        }


        /// <summary>
        /// Put a task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tasks>> PostTasks(Tasks task)
        {
            // var tasks = await _context.Tasks.FindAsync(id, todoId);
            task.User = _tasksService.UserExists(task.UserId);
            task.Todo = _tasksService.TodoExists(task.TodoId);
            return Ok(await _tasksService.CreateTaskAsync(task));
        }


        /// <summary>
        /// delete a task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todoId"></param>
        /// <returns></returns>
        // DELETE: api/Tasks/5
        [HttpDelete]
        public async Task<IActionResult> DeleteTasks(long id, long todoId)
        {
            var tasks = await _context.Tasks.FindAsync(id, todoId);
            if (tasks == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(tasks);
            await _context.SaveChangesAsync();

            return NoContent();
        }

       

    }
}
