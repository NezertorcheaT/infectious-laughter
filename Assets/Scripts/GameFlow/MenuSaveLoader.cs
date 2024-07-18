using Levels.StoryNodes;
using Saving;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameFlow
{
    public class MenuSaveLoader
    {
        [Inject] private LevelManager levelManager;
        [Inject] private SessionFactory sessionFactory;

        public void LoadSave(string id)
        {
            sessionFactory.LoadSession(id);
            levelManager.SetLevel(sessionFactory.Current[NewGameStarter.SavedLevelKey].Value as string);
            SceneManager.LoadScene(levelManager.CurrentLevel.Scene);
        }
    }
}