using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sprinti.Domain;

namespace Sprinti.Confirmation;

public static class JsonConverters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy-mm-dd HH:mm:ss";

        public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
        {
            writer.WriteStringValue(date.ToString(Format));
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString() ?? string.Empty, Format, DateTimeFormatInfo.InvariantInfo);
        }
    }

    public class ConfigDictionaryConverter : JsonConverter<SortedDictionary<int, Color>>
    {
        public override SortedDictionary<int, Color>? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, SortedDictionary<int, Color> value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var pair in value)
            {
                writer.WritePropertyName(pair.Key.ToString());
                JsonSerializer.Serialize(writer, pair.Value.Map(), options);
            }

            writer.WriteEndObject();
        }
    }
}