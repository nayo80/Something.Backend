using Shared.DTO;

namespace Products.Application.Dtos.Cars;

public record UpdateCarDto : BaseDto
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public DateTime? ReleaseDate { get; set; }
}