using System;
using System.Collections.Generic;
using System.Linq;
using Saving;
using UnityEngine;

namespace CustomHelper
{
    public static partial class Helper
    {
        /// <summary>
        /// кароч комбинация Select и Where<br/>
        /// сначала выполняется Where по TSource<br/>
        /// потом выполняется выборка<br/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TResult> WhereSelect<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector,
            Func<TSource, bool> predicate
        ) => source.Where(predicate).Select(selector);

        /// <summary>
        /// кароч комбинация Select и Where<br/>
        /// сначала выполняется выборка<br/>
        /// потом выполняется Where по TResult<br/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TResult> SelectWhere<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector,
            Func<TResult, bool> predicate
        ) => source.Select(selector).Where(predicate);

        private static readonly IReadOnlyCollection<SavingKey> SavedKeysFields = typeof(SavedKeys)
            .GetFields()
            .SelectWhere(
                i => i.GetValue(null) as SavingKey,
                i => i is not null
            )
            .ToArray();

        /// <summary>
        /// желательно проверять сохранения на предмет отсутствующих ключей
        /// </summary>
        /// <param name="session">сессия для проверки</param>
        /// <returns></returns>
        public static bool SaveCorruptCheck(this Session session)
        {
            if (SavedKeysFields.All(session.Has)) return true;
            Debug.LogError("Ваше сохранение не имеет определённых ключей! создайте новое!");
            return false;
        }

        public static void InitializeWithDefaultsIfNot(this Session session)
        {
            foreach (var savingKey in SavedKeysFields)
            {
                if (session.Has(savingKey)) continue;
                session.Add(savingKey.Default, savingKey.Type, savingKey.Key);
            }
        }

        public static void InitializeWithDefaults(this Session session)
        {
            foreach (var savingKey in SavedKeysFields)
                session.Add(savingKey.Default, savingKey.Type, savingKey.Key);
        }

        /// <summary>
        /// используя строку, как ключ, найти соответствующий SavingKey
        /// </summary>
        /// <param name="key">зарегистрированный ключ</param>
        /// <returns></returns>
        public static SavingKey GetSavedKey(string key) =>
            SavedKeysFields.FirstOrDefault(savingKey => string.Equals(savingKey.Key, key));
    }
}