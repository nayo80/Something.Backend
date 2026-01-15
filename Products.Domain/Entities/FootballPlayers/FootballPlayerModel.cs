using Shared.DTO;

namespace Products.Domain.Entities.FootballPlayers;

public class FootballPlayerModel : BaseModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FootballClub { get; set; } = string.Empty;
}