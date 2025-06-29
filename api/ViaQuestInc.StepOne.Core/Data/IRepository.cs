using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ViaQuestInc.StepOne.Core.Data;

public interface IRepository<TContext> where TContext : DbContext
{
    /// <summary>
    /// Used to retrieve all entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="includes">Navigation properties to include.</param>
    /// <returns>IQueryable of all entities</returns>
    IQueryable<T> All<T>(params Expression<Func<T, object>>[]? includes) where T : class;

    /// <summary>
    /// Used to retrieve all entities of type <typeparamref name="T"/>, including a property that is a collection
    /// and you need to include the collection's children.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="includes">Array of linked entities to include, using namespace (dot) notation</param>
    /// <returns>IQueryable of all entities</returns>
    IQueryable<T> AllWithChildren<T>(params string[]? includes) where T : class;

    /// <summary>
    /// Used to retrieve a single (first) entity of type <typeparamref name="T"/>, for which the Expression
    /// parameter "<paramref name="expression"/>" evaluates true.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="expression">Expression to specify which entity to get</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Single entity of type <typeparamref name="T"/> for which "<paramref name="expression"/>" evaluates
    /// true.</returns>
    Task<T?> GetAsync<T>(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Used to retrieve a single (first) entity of type <typeparamref name="T"/>, for which the Expression
    /// parameter "<paramref name="expression"/>" evaluates true.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="expression">Expression to specify which entity to get</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="includes">Navigation properties to include.</param>
    /// <returns>Single entity of type <typeparamref name="T"/> for which "<paramref name="expression"/>" evaluates
    /// true, including linked entities specified by "<paramref name="includes"/>"</returns>
    Task<T?> GetWithChildrenAsync<T>(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params string[]? includes) where T : class;

    /// <summary>
    /// Used to retrieve a single (first) entity of type <typeparamref name="T"/>, for which the Expression
    /// parameter "<paramref name="expression"/>" evaluates true.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="expression">Expression to specify which entity to get</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="includes">Navigation properties to include.</param>
    /// <returns>Single entity of type <typeparamref name="T"/> for which "<paramref name="expression"/>" evaluates
    /// true, including linked entities specified by "<paramref name="includes"/>"</returns>
    Task<T?> GetWithChildrenAsync<T>(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[]? includes) where T : class;

    /// <summary>
    /// Used to retrieve a single (first) entity of type <typeparamref name="T"/> with primary key value(s)
    /// matching <paramref name="keyValues"/>.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// /// <param name="key">Primary key used to locate entity</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Single entity of type <typeparamref name="T"/> which matches the primary key value.</returns>
    Task<T?> FindAsync<T>(object key, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Used to retrieve any entities of type <typeparamref name="T"/> for which the Expression
    /// "<paramref name="expression"/>" evaluates true.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="expression">An expression to use to filter entities.</param>
    /// <param name="includes">Navigation properties to include.</param>
    /// <returns>List&lt;<typeparamref name="T"/>&gt; of all entities for which the Expression
    /// "<paramref name="expression"/>" evaluates true</returns>
    IQueryable<T> Filter<T>(
        Expression<Func<T, bool>> expression,
        params Expression<Func<T, object>>[]? includes) where T : class;

    /// <summary>
    /// Used to retrieve any entities of type <typeparamref name="T"/> including  for which the Expression
    /// "<paramref name="predicate"/>" evaluates true, including a property that is a collection and you need to
    /// include the collection's children.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="predicate">An expression to use to filter entities.</param>
    /// <param name="includes">Array of linked entities to include, using namespace (dot) notation</param>
    /// <returns>IQueryable of all entities for which the Expression "<paramref name="predicate"/>"
    /// evaluates true</returns>
    IQueryable<T> FilterWithChildren<T>(
        Expression<Func<T, bool>> predicate,
        params string[]? includes) where T : class;

    /// <summary>
    /// Used to determine if the database contains any entities of type <typeparamref name="T"/> for which the
    /// Expression "<paramref name="predicate"/>" evaluates true.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="predicate">Expression to search entities</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>True if "<paramref name="predicate"/>" evaluates true for any entity of type
    /// <typeparamref name="T"/> in the database</returns>
    Task<bool> ContainsAsync<T>(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Begins tracking a new entity of type <typeparamref name="T"/> without persisting to the database.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="t">New Entity</param>
    /// <returns>The entity as it exists in the database (will include Id)</returns>
    T CreateNoSave<T>(T t) where T : class;

    /// <summary>
    /// Adds a new entity of type <typeparamref name="T"/> to the database.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="entity">New Entity</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The entity as it exists in the database (will include Id)</returns>
    Task<T> CreateAsync<T>(T entity, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Adds the entities in <paramref name="entities"/> to the database.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="entities">The list of objects</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Void Task</returns>
    Task CreateRangeAsync<T>(IList<T> entities, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Deletes entity <paramref name="entity"/> from the database.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="entity">Entity to delete</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Number of rows deleted (includes rows deleted by linked entities, SQL triggers, etc...)</returns>
    Task<int> DeleteAsync<T>(T? entity, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Deletes all entities of type <typeparam name="T"></typeparam> from the database for which the Expression
    /// "<paramref name="predicate"/>" evaluates true.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="predicate">Expression to search entities</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Number of rows deleted (includes entities, rows deleted by linked entities, SQL triggers, etc...)
    /// </returns>
    Task<int> DeleteAsync<T>(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Bulk deletes the supplied entities.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="entities">The entities to be deleted</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    Task DeleteRangeAsync<T>(IList<T> entities, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Deletes all records from the table which corresponds to <typeparam name="T"></typeparam>.
    /// </summary>
    /// <typeparam name="T">Entity type for the table to clear</typeparam>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Void Task</returns>
    Task ClearAsync<T>(CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Updates an existing entity of type <typeparamref name="T"/> in the database.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="entity">New Entity</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Number of rows updated (includes entities, rows updated by linked entities, SQL triggers, etc...)
    /// </returns>
    Task<int> UpdateAsync<T>(T entity, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Bulk updates a list of objects.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="entities">The list of entities</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Number of rows updated</returns>
    Task<int> UpdateRangeAsync<T>(IList<T> entities, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Executes a SQL stored procedure with parameters <paramref name="sqlParams"/>.
    /// </summary>
    /// <param name="procedureCommand">Name of the SQL stored procedure to execute</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="sqlParams">SQL Parameter to pass to the stored procedure</param>
    /// <returns>Void Task</returns>
    Task ExecuteProcedureAsync(
        string procedureCommand,
        CancellationToken cancellationToken,
        params SqlParameter[] sqlParams);

    /// <summary>
    /// Used to execute a raw SQL query, and map the results to a list of type <typeparamref name="T"/> objects.
    /// </summary>
    /// <typeparam name="T">Type to map each query result to</typeparam>
    /// <param name="query">Raw SQL query string</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>List&lt;<typeparamref name="T"/>&gt; of mapped objects</returns>
    Task<List<T>> RawSqlQueryAsync<T>(string query, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Executes a raw SQL statement.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken);

    /// <summary>
    /// Saves all changes to any entities into underlying database.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Void Task</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets the table name for entity <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    string GetTableName<T>() where T : class;

    /// <summary>
    /// Determines whether a DbSet has any entities.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<bool> AnyAsync<T>(CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Gets all navigation properties for <paramref name="t"/>.
    /// </summary>
    IEnumerable<INavigation> GetNavigationProperties(Type t);

    /// <summary>
    /// Validates the requested includes against <typeparam name="T"></typeparam>. If a wildcard ('*', 'all') was
    /// supplied, all navigation property names are returned. If an invalid includes is found, an exception is
    /// thrown.
    /// </summary>
    /// <param name="requestedIncludes">The navigation property names to validate.</param>
    /// <typeparam name="T">The type against which <param name="requestedIncludes"></param> are validated.
    /// </typeparam>
    /// <returns>A collection of navigation property names based on the
    /// provided <see cref="requestedIncludes"/>.</returns>
    IEnumerable<string> ValidateRequestedIncludes<T>(string[]? requestedIncludes) where T : class;

    /// <summary>
    /// Disallow the updating of navigation properties by setting them to null.
    /// </summary>
    /// <param name="entity"></param>
    T StripNavigationProperties<T>(T entity) where T : class;
}