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

            sessionFactory.Current.Add(levelManager.CurrentLevel.ID, Helper.SavedLevelKey);
            sessionFactory.Current.Add(JsonUtility.ToJson(playerInventory), Helper.SavedPlayerInventoryKey);
            sessionFactory.Current.Add(playerInitialHp, Helper.SavedPlayerHpKey);
            sessionFactory.Current.Add(playerInitialAddictiveHp, Helper.SavedPlayerAddictiveHpKey);
            sessionFactory.Current.Add(playerInitialMaxHp, Helper.SavedPlayerMaxHpKey);
            sessionFactory.Current.Add(playerInitialMaxAddictiveHp, Helper.SavedPlayerMaxAddictiveHpKey);
            sessionFactory.Current.Add(playerInitialGarbage, Helper.SavedPlayerGarbageKey);
            sessionFactory.Current.Add(Guid.NewGuid().ToString(), Helper.SavedSeed);

            sessionFactory.SaveCurrentSession();

            saveLoader.LoadSave(sessionFactory.Current.ID);
        }
    }
}

namespace CustomHelper
{
    public static partial class Helper
    {
        public static readonly string SavedLevelKey = "game.saved_level_key";
        public static readonly string SavedPlayerInventoryKey = "game.saved_player_inventory_key";
        public static readonly string SavedPlayerGarbageKey = "game.saved_player_garbage_key";
        public static readonly string SavedPlayerHpKey = "game.saved_player_hp_key";
        public static readonly string SavedPlayerAddictiveHpKey = "game.saved_player_addictive_hp_key";
        public static readonly string SavedPlayerMaxHpKey = "game.saved_player_max_hp_key";
        public static readonly string SavedPlayerMaxAddictiveHpKey = "game.saved_player_max_addictive_hp_key";
        public static readonly string SavedSeed = "game.saved_seed";

        /// <summary>
        /// желаельно проверять сохранения на предмет отсутствующих ключей.<br/>
        /// создавая новый ключ, запишите его в этот метод, чтоб он тоже проходил проверку
        /// </summary>
        /// <param name="session">сессия для проверки</param>
        /// <returns></returns>
        public static bool SaveCorruptCheck(Session session)
        {
            var has =
                session.Has(SavedLevelKey) &&
                session.Has(SavedPlayerInventoryKey) &&
                session.Has(SavedPlayerHpKey) &&
                session.Has(SavedPlayerAddictiveHpKey) &&
                session.Has(SavedPlayerMaxHpKey) &&
                session.Has(SavedPlayerMaxAddictiveHpKey) &&
                session.Has(SavedPlayerGarbageKey) &&
                session.Has(SavedSeed) &&
                true;
            if (!has)
                Debug.LogError("Ваше сохранение не имеет определённых ключей! создайте новое!");
            return has;
        }
    }
}