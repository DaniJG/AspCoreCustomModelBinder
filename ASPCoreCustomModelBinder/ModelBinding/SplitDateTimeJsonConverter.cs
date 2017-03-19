using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreCustomModelBinder.ModelBinding
{
    public class SplitDateTimeJsonConverter : JsonConverter
    {
        private readonly DateTimeConverterBase fallbackConverter;
        public SplitDateTimeJsonConverter(DateTimeConverterBase fallbackConverter)
        {
            this.fallbackConverter = fallbackConverter;
        }

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Make sure we have an object with date and time properties
            // Example: { date: "01/01/2017", time: "11:35" }
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartObject) return fallbackConverter.ReadJson(reader, objectType, existingValue, serializer);
            var jObject = JObject.Load(reader);
            if (jObject["date"] == null || jObject["time"] == null) return fallbackConverter.ReadJson(reader, objectType, existingValue, serializer);

            // Extract and parse the separated date and time values 
            // NOTE: You might want a stronger/smarter handling of locales, formats and cultures
            DateTime.TryParseExact(
                jObject["date"].Value<string>(),
                "d",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsedDateValue);
            DateTime.TryParseExact(
                jObject["time"].Value<string>(),
                "t",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal,
                out var parsedTimeValue);

            // Combine into single DateTime as the end result
            return new DateTime(parsedDateValue.Year,
                            parsedDateValue.Month,
                            parsedDateValue.Day,
                            parsedTimeValue.Hour,
                            parsedTimeValue.Minute,
                            parsedTimeValue.Second);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
