namespace ViaQuestInc.StepOne.Core.Data;

public abstract class SearchRequestBase
{
    public const int MaxLimit = 500;

    private int limit;

    public SearchRequestBase()
        : this(false, 1, MaxLimit, null, false, Array.Empty<string>())
    {
    }

    public SearchRequestBase(
        bool desc,
        int page,
        int limit,
        string? sortBy,
        bool disableDefaultSortById,
        string[] includes)
    {
        Desc = desc;
        Page = page;
        Limit = limit;
        SortBy = sortBy;
        DisableDefaultSortById = disableDefaultSortById;
        Includes = includes;
    }

    /// <summary>
    /// Reflects whether the results should be sorted in descending order.
    /// </summary>
    public bool Desc { get; set; }

    /// <summary>
    /// The page index being requested.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// The requested page size.
    /// </summary>
    public int Limit
    {
        get => limit;
        set => limit = SinglePage
            ? int.MaxValue
            : Math.Min(value, MaxLimit);
    }

    /// <summary>
    /// The field by which the results should be sorted.  If null or empty, the
    /// entity's ID field will be used.
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// If true, a null SortBy will not be set to id.
    /// </summary>
    public bool DisableDefaultSortById { get; set; }

    /// <summary>
    /// The navigation properties to include in <see cref="SearchResponse{T}"/>.
    /// </summary>
    public string[] Includes { get; set; }

    /// <summary>
    /// If true the <see cref="Limit"/> is ignored and all results are returned in a single page.
    /// </summary>
    public bool SinglePage { get; set; }
}