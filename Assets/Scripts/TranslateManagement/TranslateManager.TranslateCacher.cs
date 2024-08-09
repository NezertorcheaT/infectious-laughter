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
                Cache = new(196);
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
                {
                    return null;
                }



                // Try get cache
                if (Cache.TryGetValue(name, out FieldCache cache))
                {
                    // Return cached value
                    return cache.GetValue();
                }


                // Try to cache
                if (CacheField(name))
                {
                    // Repeat the method to take the cached field
                    return Get(name);
                }

                else // Throw Exception if caching failed
                    throw new ArgumentException($"Translation string with {name} name not founded");
            }


            /// <summary>
            /// Tries to cache a field and returns false if the field is already cached
            /// </summary>
            public static bool CacheField(string name)
            {
                return Cache.TryAdd(name, new FieldCache(name));
            }


            /// <summary>
            /// Class for optimized caching of translation strings
            /// </summary>
            public class FieldCache
            {
                public readonly FieldInfo Field;

                private string Value;
                private bool IsDirty;


                public FieldCache(string name)
                {
                    Field = typeof(Translation).GetField(name);
                    RebuildData();
                }

                /// <summary>
                /// Marks an object as dirty so that it will be rebuilt the next <seealso cref="GetValue"/> is called
                /// </summary>
                public void SetDirtyData() => IsDirty = true;

                /// <summary>
                /// Clears value
                /// </summary>
                public void ClearData() => Value = null;

                /// <summary>
                /// Re-searches for value using <seealso cref="Field"/>
                /// </summary>
                public void RebuildData()
                {
                    Value = Field.GetValue(TranslateManager.Translation) as string;
                    IsDirty = false;
                }



                public string GetValue()
                {
                    if (IsDirty)
                        RebuildData();

                    return Value;
                }
                    
            }
        }
    }
}
