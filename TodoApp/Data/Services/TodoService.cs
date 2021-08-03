using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NodaTime;
using TodoApp.Data.Sorting;
using TodoApp.Database.Data;
using TodoApp.Database.Models;

namespace TodoApp.Data.Services
{
    public class TodoService : ITodoService
    {
        private readonly UserService _userService;
        private readonly TodoContext _todoContext;
        private readonly ZonedClock _clock;
        private readonly ILogger _logger;

        public TodoService(UserService userService, TodoContext todoContext, ZonedClock clock, ILogger<TodoService> logger)
        {
            _userService = userService;
            _todoContext = todoContext;
            _clock = clock;
            _logger = logger;
        }

        public async Task<List<TodoItem>> GetTodoItemsAsync(
            Expression<Func<TodoItem, bool>> listFilter = null, 
            (TodoItemSortOrder column, ListSortDirection direction)? sortOrder = null)
        {
            string userId = _userService.GetUserId();
            IQueryable<TodoItem> query = _todoContext.TodoItems.Where(todoItem => todoItem.UserId == userId);
            if(listFilter != null) query = query.Where(listFilter);
            if (sortOrder != null) query = query.OrderBy(sortOrder.Value.column, sortOrder.Value.direction);

            return await query.ToListAsync();
        }

        public async Task DeleteTodoItemAsync(int id)
        {
            var todoItem = await _todoContext.TodoItems.SingleOrDefaultAsync(i => i.Id == id);
            GuardAgainstWrongUser(todoItem);

            if (todoItem != null)
            {
                _todoContext.TodoItems.Remove(todoItem);
                await _todoContext.SaveChangesAsync();
            }
        }

        public async Task AddTodoItemAsync(TodoItem newItem)
        {
            newItem.SetCreated(_clock.GetCurrentZonedDateTime());
            newItem.UserId = _userService.GetUserId();
            _todoContext.TodoItems.Add(newItem);
            await _todoContext.SaveChangesAsync();
        }

        public async Task UpdateTodoItemAsync(TodoItem todoItem)
        {
            GuardAgainstWrongUser(todoItem);
            _todoContext.TodoItems.Update(todoItem);
            await _todoContext.SaveChangesAsync();
        }

        private void GuardAgainstWrongUser(TodoItem todoItem)
        {
            string userId = _userService.GetUserId();
            if (todoItem != null && todoItem.UserId != userId)
            {
                _logger.LogError("User {userId} is trying to access todo item {todoItemId} which it does not own.", userId, todoItem.Id);
                throw new InvalidOperationException($"User {userId} is trying to access todo item {todoItem.Id} which it does not own.");
            }
        }
    }
}
