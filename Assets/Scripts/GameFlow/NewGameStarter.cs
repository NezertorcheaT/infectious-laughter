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

        public static readonly string SavedLevelKey = "game.saved_level_key";
        public static readonly string SavedPlayerInventoryKey = "game.saved_player_inventory_key";
        public static readonly string SavedPlayerGarbageKey = "game.saved_player_garbage_key";
        public static readonly string SavedPlayerHpKey = "game.saved_player_hp_key";
        public static readonly string SavedPlayerAddictiveHpKey = "game.saved_player_addictive_hp_key";
        public static readonly string SavedPlayerMaxHpKey = "game.saved_player_max_hp_key";
        public static readonly string SavedPlayerMaxAddictiveHpKey = "game.saved_player_max_addictive_hp_key";

        public void StartNewGame()
        {
            levelManager.Reset();
            sessionFactory.NewSession();
            playerInventory.ClearInventory();

            sessionFactory.Current.Add(levelManager.CurrentLevel.ID, SavedLevelKey);
            sessionFactory.Current.Add(JsonUtility.ToJson(playerInventory), SavedPlayerInventoryKey);
            sessionFactory.Current.Add(playerInitialHp, SavedPlayerHpKey);
            sessionFactory.Current.Add(playerInitialAddictiveHp, SavedPlayerAddictiveHpKey);
            sessionFactory.Current.Add(playerInitialMaxHp, SavedPlayerMaxHpKey);
            sessionFactory.Current.Add(playerInitialMaxAddictiveHp, SavedPlayerMaxAddictiveHpKey);
            sessionFactory.Current.Add(playerInitialGarbage, SavedPlayerGarbageKey);

            sessionFactory.SaveCurrentSession();

            saveLoader.LoadSave(sessionFactory.Current.ID);
        }
    }
}