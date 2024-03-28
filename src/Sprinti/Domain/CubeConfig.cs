using System.Text.Json.Serialization;
using static Sprinti.Confirmation.JsonConverters;

namespace Sprinti.Domain;

public class CubeConfig
{
    [JsonPropertyName("time")]
    [JsonConverter(typeof(DateTimeConverter))]
    public required DateTime Time { get; set; }

    [JsonPropertyName("config")]
    public required SortedDictionary<int, Color> Config { get; set; }
}