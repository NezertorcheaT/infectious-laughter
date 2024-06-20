using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

            [JsonConstructor]
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

        public object this[string key] => _container[key];

        public void Add<T>(T content, string key) => Add(content, typeof(T), key);

        public void Add(object content, Type type, string key)
        {
            if (_container.ContainsKey(key)) throw new ArithmeticException("key already exists");
            _container.Add(key, new SessionContent(content, type));
        }

        public void Forget(string key)
        {
            _container.Remove(key);
        }

        public string Convert { get; }

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
    }
}