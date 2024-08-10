using System;
using System.Collections.Generic;
using System.Reflection;

namespace TranslateManagement
{
    public static partial class TranslateManager
    {
        private static class TranslateCacher
        {
            private readonly static Dictionary<string, FieldCache> Cache;

            static TranslateCacher()
            {
                Cache = new Dictionary<string, FieldCache>(196);
            }


            public static void RebuildData()
            {
                foreach (FieldCache cache in Cache.Values)
                {
                    cache.SetDirtyData();
                }
            }


            /// <summary>
            /// Optimized takes the cached value by field name
            /// </summary>
            public static string Get(string name)
            {
                // Validate
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                // Try get cache and return cached value
                if (Cache.TryGetValue(name, out var cache))
                    return cache.GetValue();

                // Try to cache and repeat the method to take the cached field
                if (CacheField(name))
                    return Get(name);

                // Throw Exception if caching failed
                throw new ArgumentException($"Translation string with {name} name not founded");
            }

            /// <summary>
            /// Tries to cache a field and returns false if the field is already cached
            /// </summary>
            public static bool CacheField(string name) => Cache.TryAdd(name, new FieldCache(name));

            /// <summary>
            /// Class for optimized caching of translation strings
            /// </summary>
            public class FieldCache
            {
                public readonly FieldInfo Field;

                private string _value;
                private bool _isDirty;


                public FieldCache(string name)
                {
                    Field = typeof(Translation).GetField(name);
                    RebuildData();
                }

                /// <summary>
                /// Marks an object as dirty so that it will be rebuilt the next <seealso cref="GetValue"/> is called
                /// </summary>
                public void SetDirtyData() => _isDirty = true;

                /// <summary>
                /// Clears value
                /// </summary>
                public void ClearData() => _value = null;

                /// <summary>
                /// Re-searches for value using <seealso cref="Field"/>
                /// </summary>
                public void RebuildData()
                {
                    _value = Field.GetValue(TranslateManager.Translation) as string;
                    _isDirty = false;
                }


                public string GetValue()
                {
                    if (_isDirty)
                        RebuildData();

                    return _value;
                }
            }
        }
    }
}