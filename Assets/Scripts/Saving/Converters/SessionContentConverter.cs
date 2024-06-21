using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using UnityEngine;

namespace Saving.Converters
{
    public class SessionContentConverter : JsonConverter<Session.Content>
    {
        public override Session.Content Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var docList = JsonDocument.ParseValue(ref reader).RootElement.EnumerateObject().ToArray();
            var typeStr = docList[0].Value.ToString();
            var contStr = docList[1].Value.ToString();
            Debug.Log((typeStr, contStr));

            var type = Type.GetType(typeStr);
            var content = typeStr == typeof(string).AssemblyQualifiedName
                ? contStr
                : JsonSerializer.Deserialize(contStr, type, options);
            Debug.Log((type, content));
            return new Session.Content(content, type);
        }

        public override void Write(Utf8JsonWriter writer, Session.Content value, JsonSerializerOptions options)
        {
            var node = new JsonObject();
            node.Add("type",
                JsonSerializer.SerializeToNode(value.Type.AssemblyQualifiedName, Session.SerializerOptions));
            node.Add("content", JsonSerializer.SerializeToNode(value.Value, value.Type, Session.SerializerOptions));
            writer.WriteRawValue(node.ToJsonString());
        }
    }
}