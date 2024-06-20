using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using UnityEngine;

namespace Saving.Converters
{
    public class SessionContentConverter : System.Text.Json.Serialization.JsonConverter<Session.Content>
    {
        public override Session.Content Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var text = JsonDocument.ParseValue(ref reader).RootElement.GetRawText();
            Debug.Log(text);
            var node = JsonNode.Parse(text)?.AsObject();
            var type = Type.GetType(node["type"].Deserialize<string>(Session.SerializerOptions));
            Debug.Log(node["content"]);
            return new Session.Content(node["content"].Deserialize(type, Session.SerializerOptions), type);
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