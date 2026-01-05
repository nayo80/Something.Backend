namespace Shared.Helpers.SortFilterHelpers;

public record BaseFilter
{
    public int Page { get; set; }
    public int Amount { get; set; }
    public string? TextFilter { get; set; }
    public bool? IsDeleted { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; } = "ASC";
    public DateTime? CreatedDateFrom { get; set; }
    public DateTime? CreatedDateTo { get; set; }
}