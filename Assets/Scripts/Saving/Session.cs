using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Saving.Converters;
using UnityEngine;

namespace Saving
{
    public class Session : IFileSaver<string>.ISavable<string>, IEnumerable<Tuple<string, Session.Content>>
    {
        public string ID { get; private set; }

        [Serializable]
        public class Content
        {
            [field: SerializeField] public object Value { get; set; }

            [field: SerializeField] public Type Type { get; set; }

            public Content(object value)
            {
                Value = value;
                Type = value.GetType();
            }

            public Content(object value, Type type)
            {
                Value = value;
                Type = type ?? value.GetType();
            }
        }

        public static JsonSerializerOptions SerializerOptions => new JsonSerializerOptions
        {
            Converters =
            {
                new Vector2Converter(),
                new Vector3Converter(),
            }
        };

        private Dictionary<string, Content> _container;

        public Session()
        {
            _container = new Dictionary<string, Content>();
            ID = Guid.NewGuid().ToString();
        }

        public Session(string id)
        {
            _container = new Dictionary<string, Content>();
            ID = id;
        }

        public Content this[string key] => _container[key];

        private void Add(Content content, string key)
        {
            if (_container.ContainsKey(key)) throw new ArithmeticException("key already exists");
            _container.Add(key, content);
        }

        public void Add<T>(T content, string key) =>
            Add(new Content(content, typeof(T)), key);

        public void Add(object content, Type type, string key) =>
            Add(new Content(content, type), key);

        public void Forget(string key)
        {
            _container.Remove(key);
        }

        private IEnumerator<Tuple<string, Content>> Enumerate()
        {
            foreach (var (key, value) in _container)
            {
                yield return new Tuple<string, Content>(key, value);
            }
        }

        IEnumerator<Tuple<string, Content>> IEnumerable<Tuple<string, Content>>.GetEnumerator() =>
            Enumerate();

        IEnumerator IEnumerable.GetEnumerator() => Enumerate();

        public static string JsonSessionIdKey => "SessionID";

        string IFileSaver<string>.ISavable<string>.Convert()
        {
            var dict = new JsonObject();
            dict.Add(JsonSessionIdKey, ID);
            foreach (var (key, value) in this)
            {
                var node = new JsonObject();
                node.Add("type", JsonSerializer.SerializeToNode(value.Type.AssemblyQualifiedName, SerializerOptions));
                try
                {
                    node.Add("content", JsonSerializer.SerializeToNode(value.Value, value.Type, SerializerOptions));
                }
                catch (JsonException)
                {
                    node.Add("content", JsonNode.Parse(JsonUtility.ToJson(value.Value)));
                }

                dict.Add(key, node);
            }

            return dict.ToJsonString(SerializerOptions);
        }

        public IFileSaver<string>.ISavable<string> Deconvert(string converted, IFileSaver<string> saver)
        {
            var dict = JsonNode.Parse(converted)?.AsObject();

            if (dict is null) throw new ArgumentException($"Converted string '{converted}' is not a Dictionary");
            if (!dict.ContainsKey(JsonSessionIdKey))
                throw new ArgumentException(
                    $"Dictionary from converted string '{converted}' does not have '{JsonSessionIdKey}' key");

            var session = new Session(dict[JsonSessionIdKey].Deserialize<string>(SerializerOptions));

            foreach (var (key, value) in dict)
            {
                if (key == JsonSessionIdKey) continue;

                var typeStr = value.AsObject()["type"]?.ToString();
                if (typeStr is null)
                    throw new ArgumentException(
                        $"Content at key '{key}' in dictionary from converted string '{converted}' not contains 'type' field and is not SessionContent and can't be deserialized");

                var contStr = value.AsObject()["content"]?.ToString();
                if (contStr is null)
                    throw new ArgumentException(
                        $"Content at key '{key}' in dictionary from converted string '{converted}' not contains 'content' field and is not SessionContent and can't be deserialized");

                var type = Type.GetType(typeStr);
                if (type is null)
                    throw new ArgumentException(
                        $"Field 'type' in content at key '{key}' in dictionary from converted string '{converted}' is not a valid Type");

                object contentObj;
                if (type.AssemblyQualifiedName != typeof(string).AssemblyQualifiedName)
                {
                    try
                    {
                        contentObj = JsonSerializer.Deserialize(contStr, type, SerializerOptions);
                    }
                    catch (JsonException)
                    {
                        contentObj = JsonUtility.FromJson(contStr, type);
                    }
                }
                else
                {
                    contentObj = contStr;
                }

                session.Add(new Content(contentObj, type), key);
            }

            return session;
        }
    }
}