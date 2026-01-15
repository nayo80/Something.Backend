using Shared.DTO;

namespace Products.Domain.Entities.Products.Cars;

public class CarModel : BaseModel
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
}