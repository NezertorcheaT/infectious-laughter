using System;
using CustomHelper;
using Levels.StoryNodes;
using Saving;
using UnityEngine;
using Zenject;

namespace GameFlow
{
    public class NewGameStarter : MonoBehaviour
    {
        [Inject] private LevelManager levelManager;
        [Inject] private SessionFactory sessionFactory;
        [Inject] private MenuSaveLoader saveLoader;
        [SerializeField] private Inventory.Inventory playerInventory;
        [SerializeField] private int playerInitialHp = 5;
        [SerializeField] private int playerInitialAddictiveHp = 0;
        [SerializeField] private int playerInitialMaxHp = 5;
        [SerializeField] private int playerInitialMaxAddictiveHp = 5;
        [SerializeField] private int playerInitialGarbage;

        public void StartNewGame()
        {
            levelManager.Reset();
            sessionFactory.NewSession();
            playerInventory.ClearInventory();

            sessionFactory.Current.InitializeWithDefaults();

            sessionFactory.Current[SavedKeys.Level].Value = levelManager.CurrentLevel.ID;
            sessionFactory.Current[SavedKeys.PlayerInventory].Value = JsonUtility.ToJson(playerInventory);
            sessionFactory.Current[SavedKeys.PlayerHp].Value = playerInitialHp;
            sessionFactory.Current[SavedKeys.PlayerAddictiveHp].Value = playerInitialAddictiveHp;
            sessionFactory.Current[SavedKeys.PlayerMaxHp].Value = playerInitialMaxHp;
            sessionFactory.Current[SavedKeys.PlayerMaxAddictiveHp].Value = playerInitialMaxAddictiveHp;
            sessionFactory.Current[SavedKeys.PlayerGarbage].Value = playerInitialGarbage;

            sessionFactory.SaveCurrentSession();

            saveLoader.LoadSave(sessionFactory.Current.ID);
        }
    }
}

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

        public static void InitializeWithDefaults(this Session session)
        {
            foreach (var field in typeof(SavedKeys).GetFields())
            {
                var savingKey = field.GetValue(null) as SavingKey;
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