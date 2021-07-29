using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoApp.Data.Models;

namespace TodoApp.Data.Services
{
    public interface ITodoService
    {
        Task AddOrUpdateTodoItemAsync(TodoItem newItem);
        Task DeleteTodoItemAsync(int id);
        Task<List<TodoItem>> GetTodoItemsAsync(Expression<Func<TodoItem, bool>> listFilter = null);
    }
}