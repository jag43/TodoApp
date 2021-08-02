using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TodoApp.Database.Models;

namespace TodoApp.Data.Sorting
{
    public class TodoItemSortOrder
    {
        public string ColumnHeading { get; }

        private readonly string _column;

        private TodoItemSortOrder(string column, string columnHeading = null)
        {
            _column = column;
            ColumnHeading = columnHeading ?? column;
        }

        private static readonly TodoItem _item = new();

        public static TodoItemSortOrder Id { get; } = new(nameof(_item.Id));
        public static TodoItemSortOrder UserId { get; } = new(nameof(_item.UserId));
        public static TodoItemSortOrder Created { get; } = new(nameof(_item.CreatedUnixTicks), "Created");
        public static TodoItemSortOrder Title { get; } = new(nameof(_item.Title));
        public static TodoItemSortOrder Due { get; } = new(nameof(_item.DueUnixTicks), "Due");
        public static TodoItemSortOrder Done { get; } = new(nameof(_item.Done));

        public override bool Equals(object obj)
        {
            return obj is TodoItemSortOrder sortOrder
                && sortOrder._column == _column;
        }

        public override int GetHashCode()
        {
            return _column.GetHashCode();
        }
    }
    public static class TodoExtensions
    {
        public static (TodoItemSortOrder Column, ListSortDirection Direction) GetNewSortOrder(
            this (TodoItemSortOrder Column, ListSortDirection Direction) previousSort,
            TodoItemSortOrder newSortColumn)
        {
            bool descending = newSortColumn == previousSort.Column && previousSort.Direction == ListSortDirection.Ascending;

            return (newSortColumn, descending ? ListSortDirection.Descending : ListSortDirection.Ascending);
        }

        public static IOrderedQueryable<TodoItem> OrderBy(
            this IQueryable<TodoItem> items, 
            TodoItemSortOrder todoItemSortOrder,
            ListSortDirection direction)
        {
            bool ascending = direction == ListSortDirection.Ascending;
            if (todoItemSortOrder == TodoItemSortOrder.Id)
            {
                return ascending ? items.OrderBy(i => i.Id) : items.OrderByDescending(i => i.Id);
            }
            else if (todoItemSortOrder == TodoItemSortOrder.UserId)
            {
                return ascending ? items.OrderBy(i => i.UserId) : items.OrderByDescending(i => i.UserId);
            }
            else if (todoItemSortOrder == TodoItemSortOrder.Created)
            {
                return ascending ? items.OrderBy(i => i.CreatedUnixTicks) : items.OrderByDescending(i => i.CreatedUnixTicks);
            }
            else if (todoItemSortOrder == TodoItemSortOrder.Title)
            {
                return ascending ? items.OrderBy(i => i.Title) : items.OrderByDescending(i => i.Title);
            }
            else if (todoItemSortOrder == TodoItemSortOrder.Due)
            {
                return ascending ? items.OrderBy(i => i.DueUnixTicks) : items.OrderByDescending(i => i.DueUnixTicks);
            }
            else if (todoItemSortOrder == TodoItemSortOrder.Done)
            {
                return ascending ? items.OrderBy(i => i.Done) : items.OrderByDescending(i => i.Done);
            }
            throw new NotImplementedException();
        }
    }
}
