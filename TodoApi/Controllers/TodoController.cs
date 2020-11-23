﻿using System.Collections.Generic;
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
    [Route("api/TodoUsers")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Get all todo with paging filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodo([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Todo
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();
            var totalRecords = await _context.User.CountAsync();
            return Ok(new PagedResponse<List<Todo>>(pagedData, validFilter.PageNumber, validFilter.PageSize));

        }

        /// <summary>
        /// Get specific User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(long id)
        {
            var todo = await _context.User.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (todo == null)
            {
                return NotFound();
            }


            return Ok(todo);
        }

        /// <summary>
        /// Change specific user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todo"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(long id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            var td = await _context.Todo.FindAsync(id);
            if (td == null)
            {
                return NotFound();
            }

            td.Description = todo.Description;
            td.Tasks = todo.Tasks;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a TodoUser.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="todo"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> CreateTodo(Todo todo)
        {
            var td = new Todo
            {
                Description = todo.Description,
                Tasks = todo.Tasks
            };

            var todoValidator = new TodoValidator();

            var result = todoValidator.Validate(td);

            if (result.IsValid)
            {
                _context.Todo.Add(td);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, td);
                  
            }

            else
            {
                return BadRequest(result.Errors);
            }
        }

        /// <summary>
        /// Deletes a specific User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        private bool TodoExists(long id) =>
             _context.Todo.Any(e => e.Id == id);

    }
}
