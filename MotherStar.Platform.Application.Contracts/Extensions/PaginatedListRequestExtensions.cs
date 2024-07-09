using RCommon;
using RCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.Extensions
{
    public static class PaginatedListRequestExtensions
    {
        /// <summary>
        /// Creates an expression from the SortBy property of objects derived from the <see cref="PaginatedListRequest"/> class. 
        /// </summary>
        /// <typeparam name="TSource">Entity that we want to attempt to find a property match to the SortBy property name.</typeparam>
        /// <param name="request">The object being extended</param>
        /// <returns>A typed Expression</returns>
        /// <remarks>This is a little hokey since we are not guaranteed a property match but it beats have to create
        /// huge switch statements which return expressions.</remarks>
        public static Expression<Func<TSource, object>> DeriveOrderByExpression<TSource>(this PaginatedListRequest request)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property
            (param, request.SortBy.ToPascalCase()), typeof(object));   //important to use the Expression.Convert
            return Expression.Lambda<Func<TSource, object>>(conversion, param);
        }
    }
}
