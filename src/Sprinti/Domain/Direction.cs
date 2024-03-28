using System.Text.Json.Serialization;
using static Sprinti.Confirmation.JsonConverters;

namespace Sprinti.Domain;

[JsonConverter(typeof(DirectionJsonConverter))]
public enum Direction
{
    Up = 0,
    Down = 1
}