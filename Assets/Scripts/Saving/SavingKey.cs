using System;

namespace Saving
{
    public class SavingKey
    {
        public string Key { get; private set; }
        public Type Type { get; private set; }
        public object Default { get; private set; }

        public static SavingKey New<T>(string key, T value)
        {
            return new SavingKey
            {
                Key = key,
                Type = typeof(T),
                Default = value
            };
        }
    }
}