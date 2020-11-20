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
        /// Get all users with paging filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoUserDTO>>> GetTodoUserDTO([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.TodoUser
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .Select(x => UserToDTO(x))
            .ToListAsync();
            var totalRecords = await _context.TodoUser.CountAsync();
            return Ok(new PagedResponse<List<TodoUserDTO>>(pagedData, validFilter.PageNumber, validFilter.PageSize));

        }

        /// <summary>
        /// Get specific User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoUserDTO>> GetTodoUserDTO(long id)
        {
            var todoUser = await _context.TodoUser.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (todoUser == null)
            {
                return NotFound();
            }

            var aux = UserToDTO(todoUser);

            return Ok(new Response<TodoUserDTO>(aux));
        }

        /// <summary>
        /// Change specific user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todoUserDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoUser(long id, TodoUserDTO todoUserDTO)
        {
            if (id != todoUserDTO.Id)
            {
                return BadRequest();
            }

            var todoUser = await _context.TodoUser.FindAsync(id);
            if (todoUser == null)
            {
                return NotFound();
            }

            todoUser.Name = todoUserDTO.Name;
            todoUser.IsComplete = todoUserDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoUserExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a TodoItem.
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
        /// <param name="todoUserDTO"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoUserDTO>> CreateTodoUser(TodoUserDTO todoUserDTO)
        {
            var todoUser = new TodoUser
            {
                IsComplete = todoUserDTO.IsComplete,
                Name = todoUserDTO.Name
            };

            var todoUserValidator = new TodoUserValidator();

            var result = todoUserValidator.Validate(todoUser);

            if (result.IsValid)
            {
                _context.TodoUser.Add(todoUser);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetTodoUserDTO),
                    new { id = todoUser.Id },
                    UserToDTO(todoUser));
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
        public async Task<IActionResult> DeleteTodoUser(long id)
        {
            var todoUser = await _context.TodoUser.FindAsync(id);

            if (todoUser == null)
            {
                return NotFound();
            }

            _context.TodoUser.Remove(todoUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoUserExists(long id) =>
             _context.TodoUser.Any(e => e.Id == id);

        private static TodoUserDTO UserToDTO(TodoUser todoUser) =>
            new TodoUserDTO
            {
                Id = todoUser.Id,
                Name = todoUser.Name,
                IsComplete = todoUser.IsComplete
            };

    }
}
