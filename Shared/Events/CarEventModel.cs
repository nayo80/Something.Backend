namespace Shared.Events;

public class CarEventModel
{
    public int CarId { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public double Price { get; set; }
}