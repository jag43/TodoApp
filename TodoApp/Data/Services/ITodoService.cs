using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoApp.Data.Sorting;
using TodoApp.Database.Models;

namespace TodoApp.Data.Services
{
    public interface ITodoService
    {
        Task AddTodoItemAsync(TodoItem newItem);
        Task UpdateTodoItemAsync(TodoItem newItem);
        Task DeleteTodoItemAsync(int id);
        Task<List<TodoItem>> GetTodoItemsAsync(
            Expression<Func<TodoItem, bool>> listFilter = null,
            (TodoItemSortOrder column, ListSortDirection direction)? sortOrder = null);
    }
}