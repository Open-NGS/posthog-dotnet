using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PostHog.Library;

namespace PostHog.Json;

using static Ensure;

internal sealed class PropertyFilterValueJsonConverter : JsonConverter<PropertyFilterValue>
{
    public override PropertyFilterValue? ReadJson(JsonReader reader, Type objectType, PropertyFilterValue? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var filterElement = serializer.Deserialize<JObject>(reader);
        return PropertyFilterValue.Create(filterElement);
    }

    public override void Write(Utf8JsonWriter writer, PropertyFilterValue value, JsonSerializerOptions options)
    {
       
    }

    public override void WriteJson(JsonWriter writer, PropertyFilterValue? value, JsonSerializer serializer)
    {
        writer = NotNull(writer);

        switch (value)
        {
            case { StringValue: { } stringValue }:
                serializer.Serialize(writer, stringValue);
                break;
            case { CohortId: { } cohortId }:
                serializer.Serialize(writer, cohortId);
                break;
            case { ListOfStrings: { } stringArray }:
                {
                    // Begin writing the JSON array
                    writer.WriteStartArray();

                    // Iterate through the list and write each string value
                    foreach (var item in stringArray)
                    {
                        serializer.Serialize(writer, item);
                    }

                    // End the JSON array
                    writer.WriteEndArray();
                    break;
                }
            case null:
                {
                    writer.WriteNull();
                    break;
                }
        }
    }
}