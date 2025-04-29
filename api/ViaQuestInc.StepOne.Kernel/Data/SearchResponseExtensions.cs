namespace ViaQuestInc.StepOne.Kernel.Data;

/// <summary>
/// Extension static factory methods for <see cref="SearchResponse{T}"/>.
/// </summary>
public static class SearchResponseExtensions
{
    /// <summary>
    /// Creates a new <see cref="SearchResponse{T}"/> from a result set,
    /// using a fluent/functional style.
    /// </summary>
    /// <param name="objects">The full result-set produced by the search operation.</param>
    /// <param name="request">The original search request.</param>
    /// <typeparam name="TResponseType">The type of objects contained in the
    /// response.</typeparam>
    /// <returns>A new search response.</returns>
    public static SearchResponse<TResponseType> ToSearchResponse<TResponseType>(
        this IEnumerable<TResponseType> objects,
        SearchRequestBase request)
    {
        return new(objects, request);
    }

    /// <summary>
    /// Creates a new <see cref="SearchResponse{T}"/> from a result set,
    /// using a fluent/functional style.
    /// </summary>
    /// <param name="objects">The already-paged results to package into the search
    /// response.</param>
    /// <param name="request">The original search request.</param>
    /// <param name="totalItems">The total number of results known.</param>
    /// <typeparam name="TResponseType">The type of objects contained in the
    /// response.</typeparam>
    /// <returns>A new search response.</returns>
    public static SearchResponse<TResponseType> ToSearchResponse<TResponseType>(
        this IEnumerable<TResponseType> objects,
        SearchRequestBase request,
        int totalItems)
    {
        return new(objects.ToArray(), request, totalItems);
    }
}