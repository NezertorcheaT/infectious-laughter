using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;

namespace Saving.Converters
{
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonUtility.FromJson<Vector3>(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(JsonUtility.ToJson(value));
        }
    }
}