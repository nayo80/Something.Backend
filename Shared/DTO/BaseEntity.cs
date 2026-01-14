namespace Shared.Dto;

public record BaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}