using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sprinti.Domain;
using Sprinti.Serial;

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

    public class ResponseStateJsonConverter : JsonConverter<ResponseState>
    {
        public override void Write(Utf8JsonWriter writer, ResponseState value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Map());
        }

        public override ResponseState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Map());
        }

        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
    public class DirectionJsonConverter : JsonConverter<Direction>
    {
        public override void Write(Utf8JsonWriter writer, Direction value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Map());
        }

        public override Direction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}