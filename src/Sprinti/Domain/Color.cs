using System.Text.Json.Serialization;
using static Sprinti.Confirmation.JsonConverters;

namespace Sprinti.Domain;

// The value represents the index of the color-depot in the default/reset state
// Do not touch unless the colors are stored in another formation
[JsonConverter(typeof(ColorJsonConverter))]
public enum Color
{
    None = 0,
    Yellow = 1,
    Blue = 2,
    Red = 3
}