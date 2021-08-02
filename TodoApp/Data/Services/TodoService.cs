using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NodaTime;
using TodoApp.Data.Sorting;
using TodoApp.Database.Data;
using TodoApp.Database.Models;

namespace TodoApp.Data.Services
{
    public class TodoService : ITodoService
    {

        private readonly TodoContext _todoContext;
        private readonly ZonedClock _clock;

        public TodoService(TodoContext todoContext, ZonedClock clock)
        {
            _todoContext = todoContext;
            _clock = clock;
        }

        public async Task<List<TodoItem>> GetTodoItemsAsync(
            Expression<Func<TodoItem, bool>> listFilter = null, 
            (TodoItemSortOrder column, ListSortDirection direction)? sortOrder = null)
        {
            IQueryable<TodoItem> query = _todoContext.TodoItems;
            if(listFilter != null) query = query.Where(listFilter);
            if (sortOrder != null) query = query.OrderBy(sortOrder.Value.column, sortOrder.Value.direction);

            return await query.ToListAsync();
        }

        public async Task DeleteTodoItemAsync(int id)
        {
            var item = await _todoContext.TodoItems.SingleOrDefaultAsync(i => i.Id == id);
            if(item != null)
            {
                _todoContext.TodoItems.Remove(item);
                await _todoContext.SaveChangesAsync();
            }
        }

        public async Task AddTodoItemAsync(TodoItem newItem)
        {
            newItem.SetCreated(_clock.GetCurrentZonedDateTime());
            newItem.UserId = "test";
            _todoContext.TodoItems.Add(newItem);
            await _todoContext.SaveChangesAsync();
        }

        public async Task UpdateTodoItemAsync(TodoItem newItem)
        {
            _todoContext.TodoItems.Update(newItem);
            await _todoContext.SaveChangesAsync();
        }
    }
}
