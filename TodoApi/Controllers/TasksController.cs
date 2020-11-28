using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Filters;
using TodoApi.Models;
using TodoApi.Wrappers;

namespace TodoApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TodoContext _context;

        public TasksController(TodoContext context)
        {
            _context = context;
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
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Tasks
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();
            return Ok(new PagedResponse<List<Tasks>>(pagedData, validFilter.PageNumber, validFilter.PageSize));

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
            var tasks = await _context.Tasks.FindAsync(id, todoId);

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
            if (id != tasks.UserId)
            {
                return BadRequest();
            }

            _context.Entry(tasks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksExists(id, todo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        /// <summary>
        /// Put a task
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tasks>> PostTasks(Tasks tasks)
        {
            _context.Tasks.Add(tasks);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TasksExists(tasks.UserId, tasks.TodoId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTasks", tasks);
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

        private bool TasksExists(long id, long todoid)
        {
            return _context.Tasks.Any(e => e.UserId == id & e.TodoId == todoid);
        }
    }
}
