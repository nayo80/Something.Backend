namespace Shared.Helpers.SortFilterHelpers;

public record PriceChangeFilter : BaseFilter
{
    public string? NumberFilter { get; set; }
    public string? UserNameFilter { get; set; }
    public string? CommentFilter { get; set; }
    public DateTime? StartingRangeDateFrom { get; set; }
    public DateTime? StartingRangeDateTo { get; set; }
}