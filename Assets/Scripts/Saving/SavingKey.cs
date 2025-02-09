using System;
using JetBrains.Annotations;

namespace Saving
{
    public readonly struct SavingKey
    {
        public readonly string Key;
        public readonly Type Type;
        public readonly object Default;

        private SavingKey([NotNull] string key, [NotNull] Type type, [CanBeNull] object defaultValue)
        {
            Key = key;
            Type = type;
            Default = defaultValue;
        }

        public static SavingKey New<T>([NotNull] string key, [CanBeNull] T value) => new(key, typeof(T), value);
    }
}