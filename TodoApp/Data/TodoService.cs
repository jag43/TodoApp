using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NodaTime;

namespace TodoApp.Data
{
    public class TodoService
    {
        private readonly string _directoryPath;
        private readonly string _filePath;
        private readonly ZonedClock _clock;

        public TodoService(ZonedClock clock)
        {
            _directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create)
                + "BlazorTest";
            _filePath = Path.Join(_directoryPath, "todo.json");
            _clock = clock;
        }

        public async Task<List<TodoItem>> GetTodoItemsAsync(Expression<Func<TodoItem, bool>> listFilter = null)
        {
            if (File.Exists(_filePath))
            {
                string json = await File.ReadAllTextAsync(_filePath);
                var list = JsonConvert.DeserializeObject<List<TodoItem>>(json);
                if (listFilter != null)
                {
                    list = list.Where(listFilter.Compile()).ToList();
                }
                return list;
            }
            return new List<TodoItem>();
        }

        public async Task DeleteTodoItemAsync(int id)
        {
            var items = await GetTodoItemsAsync();
            var itemToDelete = items.SingleOrDefault(t => t.Id == id);
            if (itemToDelete != null)
            {
                items.Remove(itemToDelete);
                await SaveItemsAsync(items);
            }
        }

        public async Task AddOrUpdateTodoItemAsync(TodoItem newItem)
        {
            var items = await GetTodoItemsAsync();
            var existingItem = items.FirstOrDefault(t => newItem.Id == t.Id);
            if(existingItem != null)
            {
                items.Remove(existingItem);
            }
            else
            {
                newItem.Id = items.Any() 
                    ? items.Max(t => t.Id) + 1
                    : 1;
                newItem.Created = _clock.GetCurrentZonedDateTime();
            }

            items.Add(newItem);

            await SaveItemsAsync(items);
        }

        private async Task SaveItemsAsync(List<TodoItem> items)
        {
            Directory.CreateDirectory(_directoryPath);

            string json = JsonConvert.SerializeObject(items.OrderBy(t => t.Id));
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
