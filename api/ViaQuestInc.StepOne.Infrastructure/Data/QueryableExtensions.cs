using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ViaQuestInc.StepOne.Infrastructure.Data;

public static class QueryableExtensions
{
    public static IQueryable<T> Includes<T>(this IQueryable<T> query, params Expression<Func<T, object>>[]? includes)
        where T : class
    {
        if (includes != null && includes.Length > 0)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include).AsNoTracking());
        }

        return query;
    }
}