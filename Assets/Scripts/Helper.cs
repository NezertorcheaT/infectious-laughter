using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Random = UnityEngine.Random;

namespace CustomHelper
{
    public static partial class Helper
    {
        /// <summary>
        /// <para>Selects a random element of an enumerable, using Unity's random</para>
        /// Forces enumeration
        /// </summary>
        public static T TakeRandom<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable.ToArray();
            return array[Random.Range(0, array.Length)];
        }

        /// <summary>
        /// <para>Selects a random element of an enumerable, using System random</para>
        /// Forces enumeration
        /// </summary>
        public static T TakeRandom<T>(this IEnumerable<T> enumerable, int seed)
        {
            var random = new System.Random(seed);
            var array = enumerable.ToArray();
            return array[random.Next(0, array.Length)];
        }

        /// <summary>
        /// <para>Selects a random element of an enumerable, using System random</para>
        /// Forces enumeration
        /// </summary>
        public static T TakeRandom<T>(this IEnumerable<T> enumerable, System.Random random)
        {
            var array = enumerable.ToArray();
            return array[random.Next(0, array.Length)];
        }

        public static IEnumerable<TResult> AsType<TResult>(this IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                if (item is TResult result)
                    yield return result;
            }
        }


        public static DropdownList<T> ToDropdownList<T>(this IEnumerable<KeyValuePair<string, T>> enumerable)
        {
            var list = new DropdownList<T>();
            foreach (var (key, value) in enumerable)
            {
                list.Add(key, value);
            }

            return list;
        }

        public static DropdownList<T> ToDropdownList<T>(this IEnumerable<(string, T)> enumerable) =>
            enumerable.Select(i => new KeyValuePair<string, T>(i.Item1, i.Item2)).ToDropdownList();

        public static DropdownList<T> ToDropdownList<T>(this IEnumerable<Tuple<string, T>> enumerable) =>
            enumerable.Select(i => new KeyValuePair<string, T>(i.Item1, i.Item2)).ToDropdownList();
    }
}