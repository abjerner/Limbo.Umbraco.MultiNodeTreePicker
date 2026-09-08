using System;
using Limbo.Umbraco.MultiNodeTreePicker.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.MultiNodeTreePicker.Json.Newtonsoft;

public class MntpItemConverterJsonConverter : JsonConverter {

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {

        if (value is MntpTypeConverter converter && !string.IsNullOrWhiteSpace(converter.Type)) {
            new JObject { { "type", converter.Type } }.WriteTo(writer);
            return;
        }

        writer.WriteNull();

    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
        switch (reader.TokenType) {
            case JsonToken.Null:
                return null;
            case JsonToken.String: {
                    string? type = reader.Value as string;
                    return string.IsNullOrWhiteSpace(type) ? null : new MntpTypeConverter(type);
                }
            case JsonToken.StartObject: {
                    string? type = JObject.Load(reader).GetString("type");
                    return string.IsNullOrWhiteSpace(type) ? null : new MntpTypeConverter(type);
                }
            default:
                throw new Exception($"Unsupported token type: {reader.TokenType}...");
        }
    }

    public override bool CanConvert(Type objectType) {
        return false;
    }

}