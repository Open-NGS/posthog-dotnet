using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PostHog.Api;
using static PostHog.Library.Ensure;
namespace PostHog.Json;

internal class FilterJsonConverter : JsonConverter<Filter>
{
    public override Filter? ReadJson(JsonReader reader, Type objectType, Filter? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var filterElement = serializer.Deserialize<JObject>(reader);
        var type = filterElement?.GetValue("type")?.ToString();

        return type switch
        {
            "person" or "group" or "cohort" => serializer.Deserialize<PropertyFilter>(reader),
            "AND" or "OR" => serializer.Deserialize<FilterSet>(reader),
            _ => throw new InvalidOperationException($"Unexpected filter type: {type}")
        };
    }

    public override void WriteJson(JsonWriter writer, Filter? value, JsonSerializer serializer)
    {
        switch (value)
        {
            case PropertyFilter propertyFilter:
                serializer.Serialize(writer, propertyFilter);
                break;
            case FilterSet filterSet:
                serializer.Serialize(writer, filterSet);
                break;
            default:
                throw new InvalidOperationException($"Unexpected filter type: {NotNull(value).GetType().Name}");
        }
    }
}