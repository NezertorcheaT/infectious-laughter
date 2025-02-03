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
        [SerializeField] private Inventory.PlayerInventory playerInventory;

        public void StartNewGame()
        {
            levelManager.Reset();
            sessionFactory.NewSession();
            playerInventory.ClearInventory();

            sessionFactory.Current.InitializeWithDefaults();

            sessionFactory.Current[SavedKeys.Level].Value = levelManager.CurrentLevel.ID;
            sessionFactory.Current[SavedKeys.PlayerInventory].Value = JsonUtility.ToJson(playerInventory);

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