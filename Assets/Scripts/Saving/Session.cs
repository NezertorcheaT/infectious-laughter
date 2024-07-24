using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Saving.Converters;
using UnityEngine;

namespace Saving
{
    /// <summary>
    /// это типа объект сохранения, вы в него записываете и из него читаете
    /// </summary>
    public class Session : IFileSaver<string>.ISavable, IEnumerable<Tuple<string, Session.Content>>
    {
        /// <summary>
        /// id текущего сохранеия
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// это типа запись в сохранении
        /// </summary>
        [Serializable]
        public class Content
        {
            /// <summary>
            /// обязательно сериализуемый объект вашей записи
            /// </summary>
            public object Value;

            /// <summary>
            /// тип объекта вашей записи
            /// </summary>
            public Type Type;

            /// <summary>
            /// это типа запись в сохранениии
            /// </summary>
            /// <param name="value">обязательно сериализуемый объект вашей записи</param>
            public Content(object value)
            {
                Value = value;
                Type = value.GetType();
            }

            /// <summary>
            /// это типа запись в сохранениии
            /// </summary>
            /// <param name="value">обязательно сериализуемый объект вашей записи</param>
            /// <param name="type">тип объекта вашей записи</param>
            public Content(object value, Type type)
            {
                Value = value;
                Type = type ?? value.GetType();
            }
        }

        /// <summary>
        /// опции преобразования в json. сюда нужно сувать кастомные конвертеры
        /// </summary>
        public static JsonSerializerOptions SerializerOptions => new JsonSerializerOptions
        {
            Converters =
            {
                new Vector2Converter(),
                new Vector3Converter(),
            },
#if UNITY_EDITOR
            WriteIndented = true,
#endif
        };

        private Dictionary<string, Content> _container;

        /// <summary>
        /// пустая сессия
        /// </summary>
        public Session()
        {
            _container = new Dictionary<string, Content>();
            ID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// пустая сессия с id
        /// </summary>
        /// <param name="id"></param>
        public Session(string id)
        {
            _container = new Dictionary<string, Content>();
            ID = id;
        }

        /// <summary>
        /// получите записанный контент по ключу
        /// </summary>
        /// <param name="key">ключ контента</param>
        public Content this[string key] => _container[key];

        /// <summary>
        /// записан ли контент по ключу
        /// </summary>
        /// <param name="key">ключ контента</param>
        /// <returns>существует ли записанный контент по ключу</returns>
        public bool Has(string key) => _container.ContainsKey(key);

        /// <summary>
        /// добавить новую запись в сессию
        /// </summary>
        /// <param name="content">контент записи</param>
        /// <param name="key">ключ записи</param>
        /// <exception cref="ArgumentException">ключ записи уже существует</exception>
        private void Add(Content content, string key)
        {
            if (_container.ContainsKey(key)) throw new ArgumentException("key already exists");
            _container.Add(key, content);
        }

        /// <summary>
        /// добавить новую запись в сессию
        /// </summary>
        /// <param name="content">объект контента записи, не сам контент</param>
        /// <param name="key">ключ записи</param>
        /// <typeparam name="T">тип объекта записи</typeparam>
        /// <exception cref="ArgumentException">ключ записи уже существует</exception>
        public void Add<T>(T content, string key) =>
            Add(new Content(content, typeof(T)), key);

        /// <summary>
        /// добавить новую запись в сессию
        /// </summary>
        /// <param name="content">объект контента записи, не сам контент</param>
        /// <param name="type">тип объекта записи</param>
        /// <param name="key">ключ записи</param>
        /// <exception cref="ArgumentException">ключ записи уже существует</exception>
        public void Add(object content, Type type, string key) =>
            Add(new Content(content, type), key);

        /// <summary>
        /// забыть запись по ключу
        /// </summary>
        /// <param name="key">ключ записи</param>
        /// <returns>забытый контент, его уже нет в сессии</returns>
        public Content Forget(string key)
        {
            var forget = this[key];
            _container.Remove(key);
            return forget;
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

        /// <summary>
        /// json ключ для id сессии
        /// </summary>
        public static string JsonSessionIdKey => "SessionID";

        string IFileSaver<string>.ISavable.Convert()
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
                catch (JsonException e)
                {
                    Debug.LogException(e);
                    node.Add("content", JsonNode.Parse(JsonUtility.ToJson(value.Value)));
                }

                dict.Add(key, node);
            }

            return dict.ToJsonString(SerializerOptions);
        }

        public IFileSaver<string>.ISavable Deconvert(string converted, IFileSaver<string> saver)
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

                var contStr = value.AsObject()["content"];
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
                    catch (JsonException e)
                    {
                        Debug.LogException(e);
                        contentObj = JsonUtility.FromJson(contStr.ToString(), type);
                    }
                }
                else
                {
                    contentObj = contStr.ToString();
                }

                session.Add(new Content(contentObj, type), key);
            }

            return session;
        }
    }
}