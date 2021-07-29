using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace TodoApp.Data.Sorting
{
    public static class ExpressionExtensions
    {
        public static string GetPropertyName<T>(this Expression<Func<T, object>> expression)
        {
            var propertyInfo = expression.GetProperty();
            return propertyInfo.Name;
        }

        public static PropertyInfo GetProperty<T>(this Expression<Func<T, object>> expression)
        {
            {
                if (expression.Body is MemberExpression memberExpression
                    && memberExpression.Member is PropertyInfo propertyInfo)
                {
                    return propertyInfo;
                }
            }

            {
                if (expression.Body is UnaryExpression unaryExpression
                    && unaryExpression.Operand is MemberExpression memberExpression
                    && memberExpression.Member is PropertyInfo propertyInfo)
                {
                    return propertyInfo;
                }
            }

            throw new InvalidOperationException("Expression is not a property expression: " + expression.Body.ToString());
        }

        public static Comparison<T> GetComparison<T>(this Expression<Func<T, object>> expression, SortOrder<T> sortOrder)
        {
            return (l, r) =>
            {
                bool ascending = sortOrder.Direction == ListSortDirection.Ascending;

                var accessor = expression.Compile();

                object left = accessor(ascending ? l : r);
                object right = accessor(ascending ? r : l);

                if (left is IComparable c1)
                {
                    return c1.CompareTo(right);
                }

                string propertyTypeName = expression.GetProperty().PropertyType.Name;
                if (propertyTypeName != "Nullable`1")
                {
                    return Compare(propertyTypeName, left, right);
                }
                else
                {
                    var genericTypeName = expression.GetProperty().PropertyType.GenericTypeArguments[0].Name;
                    return Compare(genericTypeName, left, right);
                }
            };
        }

        private static int Compare(string propertyTypeName, object left, object right)
        {
            switch (propertyTypeName)
            {
                case nameof(ZonedDateTime):
                    return ZonedDateTime.Comparer.Instant.Compare(
                        ((ZonedDateTime?)left).GetValueOrDefault(), 
                        ((ZonedDateTime?)right).GetValueOrDefault());
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
