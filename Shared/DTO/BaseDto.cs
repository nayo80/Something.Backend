using System.Text.Json.Serialization;

namespace Shared.DTO;

public record BaseDto
{
    [JsonPropertyName("i")] public int Id { get; set; }
}