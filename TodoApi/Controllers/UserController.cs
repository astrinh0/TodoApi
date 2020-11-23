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
    public class UserController : ControllerBase
    {
        private readonly TodoContext _context;

        public UserController(TodoContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Get all users with paging filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUserDTO([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.User
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .Select(x => UserToDTO(x))
            .ToListAsync();
            var totalRecords = await _context.User.CountAsync();
            return Ok(new PagedResponse<List<UserDTO>>(pagedData, validFilter.PageNumber, validFilter.PageSize));

        }

        /// <summary>
        /// Get specific User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserDTO(long id)
        {
            var todoUser = await _context.User.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (todoUser == null)
            {
                return NotFound();
            }

            var aux = UserToDTO(todoUser);

            return Ok(new Response<UserDTO>(aux));
        }

        /// <summary>
        /// Change specific user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(long id, UserDTO UserDTO)
        {
            if (id != UserDTO.Id)
            {
                return BadRequest();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = UserDTO.Name;
            user.Job = UserDTO.Job;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserExists(id))
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
        /// <param name="UserDTO"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> CreateUser(UserDTO UserDTO)
        {
            var user = new User
            {
                Job = UserDTO.Job,
                Name = UserDTO.Name
            };

            var userValidator = new UserValidator();

            var result = userValidator.Validate(user);

            if (result.IsValid)
            {
                _context.User.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetUserDTO),
                    new { id = user.Id },
                    UserToDTO(user));
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
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id) =>
             _context.User.Any(e => e.Id == id);

        private static UserDTO UserToDTO(User user) =>
            new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Job = user.Job
            };

    }
}
