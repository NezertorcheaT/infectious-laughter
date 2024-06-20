using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using UnityEngine;

namespace Saving.Converters
{
    public class Vector2Converter : System.Text.Json.Serialization.JsonConverter<Vector2> 
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var node = JsonNode.Parse(reader.GetString()!)?.AsArray();
            var vector = new Vector2(node[0].Deserialize<float>(), node[1].Deserialize<float>());
            return vector;
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(JsonSerializer.Serialize(new[] {value.x, value.y}, options));
        }
    }
}