using System;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodosService : ITodosService
    {
        private readonly ITodoRepository _todoRepository;
        public TodosService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }
        public async Task<Todo> GetTodoByIdAsync(long id)
        {
            return await _todoRepository.GetByIdAsync(id);
        }
    }
}