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

#if UNITY_EDITOR
        public static void EditorNewGame(
            LevelManager levelManager,
            SessionFactory sessionFactory
        )
        {
            levelManager.Reset();
            sessionFactory.NewSession();
            sessionFactory.Current.InitializeWithDefaults();
            sessionFactory.SaveCurrentSession();
        }
#endif
    }
}