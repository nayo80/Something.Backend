using Shared.DTO;

namespace Products.Domain.Entities.Cars;

public class CarModel : BaseModel
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public double Price { get; set; }
}