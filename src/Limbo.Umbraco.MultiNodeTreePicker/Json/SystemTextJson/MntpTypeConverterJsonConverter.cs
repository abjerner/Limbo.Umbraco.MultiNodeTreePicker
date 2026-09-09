using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Limbo.Umbraco.MultiNodeTreePicker.Models;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.MultiNodeTreePicker.Json.SystemTextJson;

/// <summary>
/// JSON converter for <see cref="MntpTypeConverter"/>. Supports both the object form (<c>{ "type": "..." }</c>) and
/// the legacy string form (<c>"..."</c>) when reading, and always writes the object form.
/// </summary>
public class MntpTypeConverterJsonConverter : JsonConverter<MntpTypeConverter> {

    public override MntpTypeConverter? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

        switch (reader.TokenType) {

            case JsonTokenType.Null:
                return null;

            case JsonTokenType.String: {
                string? type = reader.GetString();
                return string.IsNullOrWhiteSpace(type) ? null : new MntpTypeConverter(MntpUtils.GetTypeAlias(type));
            }

            case JsonTokenType.StartObject: {
                using JsonDocument document = JsonDocument.ParseValue(ref reader);
                string? type = null;
                foreach (JsonProperty property in document.RootElement.EnumerateObject()) {
                    // "key" was the property name used by very early versions of the package
                    if (!property.NameEquals("type") && !property.NameEquals("key")) continue;
                    if (property.Value.ValueKind != JsonValueKind.String) continue;
                    type = property.Value.GetString();
                    if (property.NameEquals("type")) break;
                }
                return string.IsNullOrWhiteSpace(type) ? null : new MntpTypeConverter(MntpUtils.GetTypeAlias(type));
            }

            default:
                throw new JsonException($"Unsupported token type: {reader.TokenType}");

        }

    }

    public override void Write(Utf8JsonWriter writer, MntpTypeConverter? value, JsonSerializerOptions options) {

        if (value is null || string.IsNullOrWhiteSpace(value.Type)) {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("type", value.Type);
        writer.WriteEndObject();

    }

}
