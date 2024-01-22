using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelData
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByQuery<T>(this IQueryable<T> query, string columnName, bool dirDesc = false)
        {
            if (dirDesc)
            {
                return EF.CompileQuery((Hotel_DbContext ctx, IQueryable<T> q, string colName) =>
                            q.OrderByDescending(CreateOrderExpression<T>(colName))
                                      )(null, query, columnName);
            }

            return EF.CompileQuery((Hotel_DbContext ctx, IQueryable<T> q, string colName) =>
                            q.OrderBy(CreateOrderExpression<T>(colName))
                                    )(null, query, columnName);
        }

        public static Task<IOrderedQueryable<T>> OrderByQueryAsync<T>(this IQueryable<T> query, string columnName, bool dirDesc = false)
        {
            if (dirDesc)
            {
                return EF.CompileAsyncQuery((DbContext ctx, IQueryable<T> q, string colName) =>
               q.OrderByDescending(CreateOrderExpression<T>(colName))
           )(null, query, columnName);
            }
                return EF.CompileAsyncQuery((DbContext ctx, IQueryable<T> q, string colName) =>
                q.OrderBy(CreateOrderExpression<T>(colName))
            )(null, query, columnName);
        }

        private static Expression<Func<T, object>> CreateOrderExpression<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);
            return lambda;
        }
    }
}
