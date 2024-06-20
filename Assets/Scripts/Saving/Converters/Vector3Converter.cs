using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using UnityEngine;

namespace Saving.Converters
{
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var node = JsonNode.Parse(reader.GetString()!)?.AsArray();
            var vector = new Vector3(node[0].Deserialize<float>(), node[1].Deserialize<float>(), node[2].Deserialize<float>());
            return vector;
        }

        public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(JsonSerializer.Serialize(new[] {value.x, value.y, value.z}, options));
        }
    }
}