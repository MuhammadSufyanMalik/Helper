using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;


namespace Helper
{
    public static class LinqHelper
    {
        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (IQueryable<T> current, Expression<Func<T, object>> include) => current.Include(include));
            }

            return query;
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            Type typeFromHandle = typeof(TSource);
            PropertyInfo property = typeFromHandle.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            if (property == null)
            {
                property = typeFromHandle.GetProperty("Id");
            }

            ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "x");
            MemberExpression body = Expression.Property(parameterExpression, property.Name);
            LambdaExpression lambdaExpression = Expression.Lambda(body, parameterExpression);
            Type typeFromHandle2 = typeof(Queryable);
            MethodInfo methodInfo = (from m in typeFromHandle2.GetMethods()
                                     where m.Name == "OrderBy" && m.IsGenericMethodDefinition
                                     select m).Where(delegate (MethodInfo m)
                                     {
                                         List<ParameterInfo> list = m.GetParameters().ToList();
                                         return list.Count == 2;
                                     }).Single();
            MethodInfo methodInfo2 = methodInfo.MakeGenericMethod(typeFromHandle, property.PropertyType);
            return (IOrderedQueryable<TSource>)methodInfo2.Invoke(methodInfo2, new object[2] { query, lambdaExpression });
        }

        public static IOrderedQueryable<TSource> OrderByDesc<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            Type typeFromHandle = typeof(TSource);
            PropertyInfo property = typeFromHandle.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            if (property == null)
            {
                property = typeFromHandle.GetProperty("Id");
            }

            ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "x");
            MemberExpression body = Expression.Property(parameterExpression, property.Name);
            LambdaExpression lambdaExpression = Expression.Lambda(body, parameterExpression);
            Type typeFromHandle2 = typeof(Queryable);
            MethodInfo methodInfo = (from m in typeFromHandle2.GetMethods()
                                     where m.Name == "OrderByDescending" && m.IsGenericMethodDefinition
                                     select m).Where(delegate (MethodInfo m)
                                     {
                                         List<ParameterInfo> list = m.GetParameters().ToList();
                                         return list.Count == 2;
                                     }).Single();
            MethodInfo methodInfo2 = methodInfo.MakeGenericMethod(typeFromHandle, property.PropertyType);
            return (IOrderedQueryable<TSource>)methodInfo2.Invoke(methodInfo2, new object[2] { query, lambdaExpression });
        }
    }
}








