using Shared.Dto;

namespace Products.Domain.Entities.Products.Cars;

public record CarModel : BaseEntity
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
}