namespace Shared.DTO;

public class PagedResponse<T> where T : class 
{
    public int Page { get; set; }
    public int Amount { get; set; }
    public IEnumerable<T>? Data { get; set; } 
}