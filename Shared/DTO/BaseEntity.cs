namespace Shared.Dto;

// BaseEntity, BaseModel ორივე იგივეა და ორივე რად გინდა? 
public record BaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}