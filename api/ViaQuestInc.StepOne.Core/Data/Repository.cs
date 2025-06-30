using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ViaQuestInc.StepOne.Core.Data;

public class Repository<TContext>(TContext context) : IRepository<TContext>
    where TContext : DbContext
{
    private const string WildcardIncludes = "*";
    private const string TerminatingWildcardIncludes = $".{WildcardIncludes}";

    public IQueryable<T> All<T>(params Expression<Func<T, object>>[]? includes)
        where T : class
    {
        var query = context.Set<T>()
            .AsQueryable();

        if (includes != null && includes.Length != 0) query = query.Includes(includes);

        return query;
    }

    public IQueryable<T> AllWithChildren<T>(params string[]? includes)
        where T : class
    {
        var validatedIncludes = ValidateRequestedIncludes<T>(includes)
            .ToArray();

        if (validatedIncludes.Length == 0)
            return context.Set<T>()
                .AsQueryable();

        var query = context.Set<T>()
            .Include(validatedIncludes.First());

        query = validatedIncludes.Skip(1)
            .Aggregate(query, (current, include) => current.Include(include));

        return query.AsQueryable();
    }

    public async Task<T?> GetAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        where T : class
    {
        return await GetWithChildrenAsync(expression, cancellationToken, Array.Empty<string>());
    }

    public async Task<T?> GetWithChildrenAsync<T>(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params string[]? includes)
        where T : class
    {
        return await FilterWithChildren(expression, includes)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> GetWithChildrenAsync<T>(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[]? includes)
        where T : class
    {
        return await All(includes)
            .SingleOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<T?> FindAsync<T>(object key, CancellationToken cancellationToken)
        where T : class
    {
        var entity = await context.FindAsync<T>([key], cancellationToken);

        if (entity != null && context.ChangeTracker.QueryTrackingBehavior == QueryTrackingBehavior.NoTracking)
            context.Entry(entity)
                .State = EntityState.Detached;

        return entity;
    }

    public IQueryable<T> Filter<T>(
        Expression<Func<T, bool>> expression,
        params Expression<Func<T, object>>[]? includes)
        where T : class
    {
        return All(includes)
            .Where(expression);
    }

    public IQueryable<T> FilterWithChildren<T>(
        Expression<Func<T, bool>> predicate,
        params string[]? includes)
        where T : class
    {
        var validatedIncludes = ValidateRequestedIncludes<T>(includes)
            .ToArray();

        if (validatedIncludes.Length == 0)
            return context.Set<T>()
                .Where(predicate)
                .AsQueryable();

        var query = context.Set<T>()
            .Include(validatedIncludes.First());

        query = validatedIncludes
            .Skip(1)
            .Aggregate(query, (current, include) => current.Include(include));

        return query.Where(predicate)
            .AsQueryable();
    }

    public async Task<bool> ContainsAsync<T>(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken)
        where T : class
    {
        return await context.Set<T>()
            .CountAsync(predicate, cancellationToken) > 0;
    }

    public T CreateNoSave<T>(T tObject)
        where T : class
    {
        return context.Set<T>()
            .Add(tObject)
            .Entity;
    }

    public async Task<T> CreateAsync<T>(T tObject, CancellationToken cancellationToken)
        where T : class
    {
        StripNavigationProperties(tObject);

        var newEntry = (await context.Set<T>()
            .AddAsync(tObject, cancellationToken)).Entity;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new("Repository Error - Creating Entity", ex);
        }
        finally
        {
            context.Entry(tObject)
                .State = EntityState.Detached;
        }

        return newEntry;
    }

    public async Task CreateRangeAsync<T>(IList<T> entities, CancellationToken cancellationToken)
        where T : class
    {
        if (!entities.Any()) return;

        try
        {
            foreach (var entity in entities) StripNavigationProperties(entity);

            await context.AddRangeAsync(entities, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            foreach (var entry in context.ChangeTracker.Entries()) entry.State = EntityState.Detached;
        }
        catch (Exception ex)
        {
            throw new("Repository Error - Creating bulk list of entities", ex);
        }
    }

    public async Task<int> DeleteAsync<T>(T? entity, CancellationToken cancellationToken)
        where T : class
    {
        if (entity == null) return 0;

        context.Entry(entity)
            .State = EntityState.Deleted;
        context.Set<T>()
            .Remove(entity);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> DeleteAsync<T>(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken)
        where T : class
    {
        var entities = Filter(predicate)
            .ToArray();

        context.Set<T>()
            .RemoveRange(entities);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync<T>(IList<T> entities, CancellationToken cancellationToken)
        where T : class
    {
        if (!entities.Any()) return;

        foreach (var entity in entities) StripNavigationProperties(entity);

        context.RemoveRange(entities);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ClearAsync<T>(CancellationToken cancellationToken)
        where T : class
    {
        try
        {
            var cmd = $"DELETE FROM {context.Model.FindEntityType(typeof(T))!.GetTableName()}";
            await context.Database.ExecuteSqlRawAsync(cmd, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new("Repository Error - Clearing Entity", ex);
        }
    }

    public async Task<int> UpdateRangeAsync<T>(IList<T> entities, CancellationToken cancellationToken)
        where T : class
    {
        int itemsUpdated;

        try
        {
            ResetChangeTracker();

            context.UpdateRange(entities);

            itemsUpdated = await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new($"Repository Error - Bulk Updating Entities of type {typeof(T)}", ex);
        }

        return itemsUpdated;
    }

    public async Task<int> UpdateAsync<T>(T entity, CancellationToken cancellationToken)
        where T : class
    {
        int itemsUpdated;
        StripNavigationProperties(entity);

        try
        {
            context.Update(entity);
            itemsUpdated = await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new("Repository Error - Updating Entity", ex);
        }
        finally
        {
            if (context.ChangeTracker.QueryTrackingBehavior == QueryTrackingBehavior.NoTracking)
                context.Entry(entity)
                    .State = EntityState.Detached;
        }

        return itemsUpdated;
    }

    public Task ExecuteProcedureAsync(
        string procedureCommand,
        CancellationToken cancellationToken,
        params SqlParameter[] sqlParams)
    {
        return context.Database.ExecuteSqlRawAsync(procedureCommand, cancellationToken, sqlParams);
    }

    public Task<List<T>> RawSqlQueryAsync<T>(string query, CancellationToken cancellationToken)
        where T : class
    {
        return context.Set<T>()
            .FromSqlRaw(query)
            .ToListAsync(cancellationToken);
    }

    public Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken)
    {
        return context.Database.ExecuteSqlRawAsync(sql, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (TaskCanceledException)
        {
        }
    }

    public IEnumerable<INavigation> GetNavigationProperties(Type t)
    {
        return t.IsGenericType
            ? GetNavigationProperties(t.GenericTypeArguments[0])
            : context.Model.FindEntityType(t)!.GetNavigations()
                .Where(x => !x.TargetEntityType.IsOwned());
    }

    public IEnumerable<string> ValidateRequestedIncludes<T>(string[]? requestedIncludes)
        where T : class
    {
        if (requestedIncludes == null || requestedIncludes.Length == 0) return Array.Empty<string>();

        var explicitRequestedIncludes =
            requestedIncludes
                .Where(r => !r.EndsWith(WildcardIncludes))
                .ToList();
        var wildcardIncludes = requestedIncludes.Except(explicitRequestedIncludes)
            .ToList();

        // Handle [WildcardIncludes] at the root of the type
        if (wildcardIncludes.Contains(WildcardIncludes))
        {
            wildcardIncludes.Remove(WildcardIncludes);
            explicitRequestedIncludes.AddRange(
                GetNavigationProperties(typeof(T))
                    .Select(n => n.Name));
        }

        // Handle nested/deep [WildcardIncludes]
        foreach (var wildcardInclude in wildcardIncludes
                     .Select(w => w.Replace(TerminatingWildcardIncludes, string.Empty)))
        {
            var propertyInfo = GetProperty(typeof(T), wildcardInclude);
            var navPropNames = GetNavigationProperties(propertyInfo!.PropertyType)
                .Select(n => n.Name)
                .Where(n => !n.Contains(typeof(T).Name)); // To prevent cycles

            explicitRequestedIncludes.AddRange(navPropNames.Select(n => $"{wildcardInclude}.{n}"));
        }

        var validIncludes = explicitRequestedIncludes.Where(r => GetProperty(typeof(T), r) != null)
            .ToArray();
        var invalidIncludes = explicitRequestedIncludes.Except(validIncludes)
            .ToArray();

        if (invalidIncludes.Any()) throw new($"Invalid includes: {string.Join(',', invalidIncludes)}");

        return validIncludes;
    }

    public T StripNavigationProperties<T>(T entity)
        where T : class
    {
        if (context.Entry(entity)
                .State != EntityState.Detached)
            context.Entry(entity)
                .State = EntityState.Detached;

        var type = typeof(T);
        var navigationProperties = GetNavigationProperties(type);

        foreach (var navigationProperty in navigationProperties)
        {
            var prop = type.GetProperty(navigationProperty.Name, BindingFlags.Public | BindingFlags.Instance);

            if (prop != null && prop.CanWrite) prop.SetValue(entity, null);
        }

        return entity;
    }

    public string GetTableName<T>()
        where T : class
    {
        return context.Model.FindEntityType(typeof(T))!.GetTableName()!;
    }

    public async Task<bool> AnyAsync<T>(CancellationToken cancellationToken)
        where T : class
    {
        return await context.Set<T>()
            .FirstOrDefaultAsync(cancellationToken) != null;
    }

    private void ResetChangeTracker()
    {
        foreach (var entry in context.ChangeTracker.Entries()
                     .ToList()) entry.State = EntityState.Detached;
    }

    /// <summary>
    /// Gets the requested property from the supplied type.
    /// </summary>
    /// <param name="baseType">The type from which the <see cref="PropertyInfo"/> will be returned.</param>
    /// <param name="propertyName">The name of the property to return.</param>
    /// <returns></returns>
    private static PropertyInfo? GetProperty(Type baseType, string propertyName)
    {
        var parts = propertyName.Split('.');
        var propertyInfo = baseType.GetProperty(parts[0]);

        // If this is a generic collection, we want the properties on T, not ICollection
        if (parts.Length > 1 && propertyInfo != null && propertyInfo.PropertyType.IsGenericType)
        {
            var genericType = propertyInfo.PropertyType.GenericTypeArguments[0];

            return GetProperty(genericType, parts[1]);
        }

        return parts.Length > 1 && propertyInfo != null
            ? GetProperty(
                propertyInfo.PropertyType,
                parts.Skip(1)
                    .Aggregate((a, i) => a + "." + i))
            : propertyInfo;
    }
}