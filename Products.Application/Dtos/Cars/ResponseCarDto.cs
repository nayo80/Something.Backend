using Shared.Dto;

namespace Products.Application.Dtos.Cars;

public record ResponseCarDto : BaseEntity
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public DateTime? ReleaseDate { get; set; }
}