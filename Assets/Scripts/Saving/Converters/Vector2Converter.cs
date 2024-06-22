using System;
using System.Text.Json;
using UnityEngine;

namespace Saving.Converters
{
    public class Vector2Converter : System.Text.Json.Serialization.JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonUtility.FromJson<Vector2>(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(JsonUtility.ToJson(value));
        }
    }
}