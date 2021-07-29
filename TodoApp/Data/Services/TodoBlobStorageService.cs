using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Data.Models;
using TodoApp.Data.Config;

namespace TodoApp.Data.Services
{
    public class TodoBlobStorageService
        : ITodoService
    {
        private readonly TodoStorage _config;

        public TodoBlobStorageService(TodoStorage config)
        {
            _config = config;
        }

        public Task AddOrUpdateTodoItemAsync(TodoItem newItem)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTodoItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TodoItem>> GetTodoItemsAsync(Expression<Func<TodoItem, bool>> listFilter = null)
        {
            throw new NotImplementedException();
        }
    }
}
