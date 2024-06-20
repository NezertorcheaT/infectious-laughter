using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;

namespace Saving
{
    public class Session : IFileSaver<string>.ISavable<string>, IEnumerable<Tuple<string, Session.SessionContent>>
    {
        public string ID { get; private set; }

        [Serializable]
        private class SessionContent
        {
            public object Content { get; private set; }
            public Type Type { get; private set; }

            [System.Text.Json.Serialization.JsonConstructor]
            public SessionContent(object content, Type type)
            {
                Content = content;
                Type = type;
            }
        }

        private Dictionary<string, SessionContent> _container;

        public Session()
        {
            _container = new Dictionary<string, SessionContent>();
            ID = Guid.NewGuid().ToString();
        }

        public Session(string id)
        {
            _container = new Dictionary<string, SessionContent>();
            ID = id;
        }

        public object this[string key] => _container[key];

        private void Add(SessionContent content, string key)
        {
            if (_container.ContainsKey(key)) throw new ArithmeticException("key already exists");
            _container.Add(key, content);
        }

        public void Add<T>(T content, string key) => Add(new SessionContent(content, typeof(T)), key);

        public void Add(object content, Type type, string key) => Add(new SessionContent(content, type), key);

        public void Forget(string key)
        {
            _container.Remove(key);
        }

        private IEnumerator<Tuple<string, SessionContent>> Enumerate()
        {
            foreach (var (key, value) in _container)
            {
                yield return new Tuple<string, SessionContent>(key, value);
            }
        }

        IEnumerator<Tuple<string, SessionContent>> IEnumerable<Tuple<string, SessionContent>>.GetEnumerator() =>
            Enumerate();

        IEnumerator IEnumerable.GetEnumerator() => Enumerate();

        public static string JsonSessionIdKey => "SessionID";

        string IFileSaver<string>.ISavable<string>.Convert()
        {
            var dict = new JsonObject();
            dict.Add(JsonSessionIdKey, ID);
            foreach (var (key, value) in this)
            {
                dict.Add(key, JsonSerializer.Serialize(value));
            }

            return dict.ToJsonString();
        }

        public IFileSaver<string>.ISavable<string> Deconvert(string converted)
        {
            var dict = JsonNode.Parse(converted)?.AsObject();

            if (dict is null) throw new ArgumentException($"Converted string '{converted}' is not a Dictionary");
            if (!dict.ContainsKey(JsonSessionIdKey))
                throw new ArgumentException(
                    $"Dictionary from converted string '{converted}' does not have '{JsonSessionIdKey}' key");

            var session = new Session(dict[JsonSessionIdKey].Deserialize<string>());

            foreach (var node in dict)
            {
                if (node.Key == JsonSessionIdKey) continue;
                var content = node.Value.Deserialize<SessionContent>();
                if (content is null)
                    throw new ArgumentException(
                        $"Content at key '{node.Key}' in dictionary from converted string '{converted}' is not SessionContent and can't be deserialized");
                session.Add(content, node.Key);
            }

            return session;
        }
    }
}