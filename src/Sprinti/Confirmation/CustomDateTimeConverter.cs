using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sprinti.Domain;

namespace Sprinti.Confirmation;

public class CustomDateTimeConverter(string format) : JsonConverter<DateTime>
{
    public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
    {
        writer.WriteStringValue(date.ToString(format));
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString() ?? string.Empty, format, DateTimeFormatInfo.InvariantInfo);
    }
}

public class CustomColorConverter() : JsonConverter<Color>
{
    public override void Write(Utf8JsonWriter writer, Color color, JsonSerializerOptions options)
    {
        writer.WriteStringValue(color.Map());
    }

    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}