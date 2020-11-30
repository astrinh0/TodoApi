using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Filters;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Wrappers;
using TodoApi.Repositories;

namespace TodoApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Todos")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly ITodosService _todosService;


        public TodosController(TodoContext context, ITodosService todosService)
        {
            _context = context;
            _todosService = todosService;
            
        }

        /// <summary>
        /// Get all todo's with pagination filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        // GET: api/Todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodo([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Todo
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();
             return Ok(new PagedResponse<List<Todo>>(pagedData, validFilter.PageNumber, validFilter.PageSize));
        }


        /// <summary>
        /// get todo by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Todos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(long id)
        {
            var todo = await _todosService.GetTodoByIdAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }


        /// <summary>
        /// Change a todo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todo"></param>
        /// <returns></returns>
        // PUT: api/Todos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(long id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
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
        /// Post a todo on database
        /// </summary>
        /// <param name="todo"></param>
        /// <returns></returns>
        // POST: api/Todos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {

            var todoValidator = new TodoValidator();

            var result = todoValidator.Validate(todo);

            if (result.IsValid)
            {
                _context.Todo.Add(todo);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
            }

            else
            {
                return BadRequest(result.Errors);
            }
        }
    


        /// <summary>
        /// delete a todo by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(long id)
        {
            var todo = await _context.Todo.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoExists(long id)
        {
            return _context.Todo.Any(e => e.Id == id);
        }
    }
}
