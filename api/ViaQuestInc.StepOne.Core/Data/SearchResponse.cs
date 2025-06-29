using System.Linq.Expressions;

namespace ViaQuestInc.StepOne.Core.Data;

public class SearchResponse<T>
{
    /// <summary>
    /// The paged/sliced results.
    /// </summary>
    public ICollection<T> PagedItems { get; }

    /// <summary>
    /// The search request that produced this search response.
    /// </summary>
    public SearchRequestBase SearchRequest { get; }

    /// <summary>
    /// The total number of results produced by the search request, prior to slicing/pagination.
    /// </summary>
    public int TotalItems { get; }

    /// <summary>
    /// The total number of pages in the result set produced by the search request.
    /// </summary>
    public int TotalPages { get; }

    public SearchResponse(IEnumerable<T> allItems, SearchRequestBase searchRequest)
        : this(allItems.ToArray().AsQueryable(), searchRequest)
    {
    }

    public SearchResponse(IQueryable<T> allItems, SearchRequestBase searchRequest)
    {
        SearchRequest = searchRequest;
        PagedItems = GetPagedItems(allItems);
        TotalItems = allItems.Count();
        TotalPages = GetTotalPages();
    }

    public SearchResponse(ICollection<T> pagedItems, SearchRequestBase searchRequest, int totalItems)
    {
        SearchRequest = searchRequest;
        PagedItems = pagedItems;
        TotalItems = totalItems;
        TotalPages = GetTotalPages();
    }

    private ICollection<T> GetPagedItems(IEnumerable<T> allItems)
    {
        // ReSharper disable once InvertIf
        if (TryGetDefaultSortProperty<T>(out var sortBy))
        {
            var selector = GetSortSelector<T>(sortBy);
            allItems = SearchRequest.Desc
                ? allItems.OrderByDescending(selector)
                : allItems.OrderBy(selector);
        }

        return allItems
            .Skip(SearchRequest.Limit * SearchRequest.Page - SearchRequest.Limit)
            .Take(SearchRequest.Limit)
            .ToArray();
    }

    private int GetTotalPages()
    {
        return Convert.ToInt32(Math.Ceiling((double)TotalItems / SearchRequest.Limit));
    }

    /// <summary>
    /// Sets the sortByProperty argument to the SortBy value of the SearchRequest object if it is set. If it is not
    /// set then it checks if the type has an Id property, if so the sortByProperty is set to the value 'Id'
    /// </summary>
    private bool TryGetDefaultSortProperty<TSource>(out string sortByProperty)
    {
        sortByProperty = SearchRequest.SortBy!;
        if (string.IsNullOrEmpty(sortByProperty) &&
            typeof(TSource).GetProperties().Any(p => p.Name == "Id") &&
            !SearchRequest.DisableDefaultSortById)
            sortByProperty = "Id";

        return !string.IsNullOrEmpty(sortByProperty);
    }

    /// <summary>
    /// Builds a keySelector intended to be use as a parameter in an OrderBy method.
    /// </summary>
    /// <param name="sortBy">The name of the property to sort on.</param>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <returns>A keySelector.</returns>
    /// <exception cref="ArgumentException">Property does not exist on the given type.</exception>
    private static Func<TSource, dynamic> GetSortSelector<TSource>(string sortBy)
    {
        var propNames = sortBy.Split('.');

        var type = typeof(TSource);
        var parameterExpression = Expression.Parameter(type, "x");

        if (!TryGetPropertyExpression(parameterExpression, type, propNames, out var property))
            throw new ArgumentException(
                $"The property '{sortBy}' does not exist on the type '{type.Name}'.",
                nameof(sortBy));

        var boxedProperty = Expression.Convert(property, typeof(object));
        var expression = Expression.Lambda(boxedProperty, parameterExpression).Compile();

        return (Func<TSource, dynamic>)expression;
    }

    /// <summary>
    /// Attempts to build a PropertyExpression based on propNames.
    /// </summary>
    private static bool TryGetPropertyExpression(
        Expression expression,
        Type type,
        IEnumerable<string> propNames,
        out Expression propertyExpression)
    {
        propertyExpression = expression;

        foreach (var name in propNames)
        {
            var propInfo = type
                .GetProperties()
                .FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (propInfo == null) return false;

            propertyExpression = Expression.Property(propertyExpression, propInfo);
            type = propInfo.PropertyType;
        }

        return true;
    }
}