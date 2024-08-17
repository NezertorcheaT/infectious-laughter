using Saving;
using UnityEngine;

namespace CustomHelper
{
    public static partial class Helper
    {
        /// <summary>
        /// желаельно проверять сохранения на предмет отсутствующих ключей
        /// </summary>
        /// <param name="session">сессия для проверки</param>
        /// <returns></returns>
        public static bool SaveCorruptCheck(this Session session)
        {
            foreach (var field in typeof(SavedKeys).GetFields())
            {
                if (session.Has(field.GetValue(null) as SavingKey)) continue;
                Debug.LogError("Ваше сохранение не имеет определённых ключей! создайте новое!");
                return false;
            }

            return true;
        }

        public static void InitializeWithDefaultsIfNot(this Session session)
        {
            foreach (var field in typeof(SavedKeys).GetFields())
            {
                if (field.GetValue(null) is SavingKey savingKey && !session.Has(savingKey))
                    session.Add(savingKey.Default, savingKey.Type, savingKey.Key);
            }
        }

        public static void InitializeWithDefaults(this Session session)
        {
            foreach (var field in typeof(SavedKeys).GetFields())
            {
                if (!(field.GetValue(null) is SavingKey savingKey)) continue;
                session.Add(savingKey.Default, savingKey.Type, savingKey.Key);
            }
        }

        /// <summary>
        /// используя строку, как ключ, найти соответствующий SavingKey
        /// </summary>
        /// <param name="key">зарегестрированный ключ</param>
        /// <returns></returns>
        public static SavingKey GetSavedKey(string key)
        {
            foreach (var field in typeof(SavedKeys).GetFields())
            {
                var sk = field.GetValue(null) as SavingKey;
                if (sk is not null && string.Equals(sk.Key, key))
                    return sk;
            }

            return null;
        }
    }
}