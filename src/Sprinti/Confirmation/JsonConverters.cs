using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

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
}