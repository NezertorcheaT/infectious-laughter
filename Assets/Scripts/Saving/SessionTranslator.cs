using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Saving
{
    public class SessionTranslator : ISerializableTranslator<string>
    {
        public string Serialize(object a)
        {
            if (!(a is Session session))
                throw new ArgumentException("'a' argument must be of type 'Session' this time");
            var dict = new JsonObject();
            foreach (var (key, value) in session)
            {
                dict.Add(key, JsonSerializer.Serialize(value));
            }

            return dict.ToJsonString();
        }

        public object Deserialize(string a)
        {
            throw new System.NotImplementedException();
        }
    }
}