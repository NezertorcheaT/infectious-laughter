using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Saving.Converters;
using UnityEngine;

namespace Saving
{
    public class Session : IFileSaver<string>.ISavable<string>, IEnumerable<Tuple<string, Session.Content>>
    {
        public string ID { get; private set; }

        [Serializable]
        [JsonConverter(typeof(SessionContentConverter))]
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
                new SessionContentConverter(),
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
                JsonNode node;
                try
                {
                    node = JsonSerializer.SerializeToNode(value, SerializerOptions);
                }
                catch (JsonException)
                {
                    node = JsonNode.Parse(JsonUtility.ToJson(value));
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

                Debug.Log((key, value));
                var content = value.Deserialize<Content>(SerializerOptions);

                if (content is null)
                    throw new ArgumentException(
                        $"Content at key '{key}' in dictionary from converted string '{converted}' is not SessionContent and can't be deserialized");
                Debug.Log((content.Type, content.Value));
                session.Add(content, key);
            }

            return session;
        }
    }
}