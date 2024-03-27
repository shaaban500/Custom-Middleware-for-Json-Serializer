using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace testAPI
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Expected string value for datetime, but found {reader.TokenType}.");
            }

            string dateString = reader.GetString();

            // Fix the datetime format before parsing
            if (DateTime.TryParse(dateString, out DateTime result))
            {
                return result;
            }
            else
            {
                // Handle invalid datetime format
                throw new JsonException($"Invalid datetime format: {dateString}");
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Implement custom serialization logic
            writer.WriteStringValue(value.ToString("dddd, MMMM dd, yyyy hh:mm:ss tt"));
        }
    }

}
