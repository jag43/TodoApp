using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace TodoApp.Data.Sorting
{
    public class SortOrder<T>
    {
        public Expression<Func<T, object>> Column { get; set; }
        public ListSortDirection Direction { get; set; }

        public SortOrder<T> GetNewSortOrder(Expression<Func<T, object>> expression)
        {
            string newColumn = expression.GetPropertyName();
            bool descending = newColumn == Column.GetPropertyName() && Direction == ListSortDirection.Ascending;

            return new SortOrder<T>
            {
                Column = expression,
                Direction = descending ? ListSortDirection.Descending : ListSortDirection.Ascending
            };
        }

        public Comparison<T> CreateComparer()
        {
            return Column.GetComparison(this);
        }
    }
}
