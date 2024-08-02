using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;

namespace CustomHelper
{
    public static partial class Helper
    {
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