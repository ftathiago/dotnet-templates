using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using WebApi.Domain.Repositories.Filters;
using WebApi.EfInfraData.Attributes;

namespace Plataforma.DotnetRestDemo.EfInfraData.Extensions
{
    public static class QueryableExtensions
    {
        private static readonly Regex _pattern =
            new(@"^([a-z|0-9]*)(\s?)([a-z]*)$", RegexOptions.IgnoreCase);

        public static IQueryable<TTableType> OrderingFrom<TTableType>(
                    this IQueryable<TTableType> queryable,
                    PaginationFilter paginatedFilter)
        {
            if (string.IsNullOrWhiteSpace(paginatedFilter.OrderBy))
            {
                return queryable;
            }

            var orderFields = paginatedFilter.OrderBy
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(order =>
                {
                    var match = _pattern.Match(order);
                    return new string[]
                    {
                        match.Groups[1].Value,
                        match.Groups[3].Value,
                    };
                })
                .ToDictionary(row => row[0], row => row[1]);

            return typeof(TTableType)
                .GetProperties()
                .Aggregate(queryable, (acc, prop) =>
                {
                    var attr = prop.GetCustomAttributes(typeof(OrderedByAttribute), true)
                        .Select(attr => attr as OrderedByAttribute)
                        .FirstOrDefault(attr => attr is not null);

                    if (attr is null)
                    {
                        return acc;
                    }

                    var ordenableProperty = orderFields.TryGetValue(attr.JsonPropertyName, out var ascDesc);
                    if (!ordenableProperty)
                    {
                        return acc;
                    }

                    if (ascDesc == "desc")
                    {
                        return acc.OrderByDescending(prop.Name);
                    }

                    return acc.OrderBy(prop.Name);
                });
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            // LAMBDA: x => x.[PropertyName]
            var parameter = Expression.Parameter(typeof(TSource), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            // REFLECTION: source.OrderBy(x => x.Property)
            var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TSource), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<TSource>)result;
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            // LAMBDA: x => x.[PropertyName]
            var parameter = Expression.Parameter(typeof(TSource), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            // REFLECTION: source.OrderBy(x => x.Property)
            var orderByDescendingMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);
            var orderByGeneric = orderByDescendingMethod.MakeGenericMethod(typeof(TSource), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<TSource>)result;
        }
    }
}
