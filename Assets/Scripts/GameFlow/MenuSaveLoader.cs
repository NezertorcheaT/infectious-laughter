using CustomHelper;
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

        /// <summary>
        /// для загрузки сохранений из меню
        /// </summary>
        /// <param name="id">айди сохранения</param>
        public void LoadSave(string id)
        {
            sessionFactory.LoadSession(id);
            sessionFactory.Current.SaveCorruptCheck();
            levelManager.SetLevel(sessionFactory.Current[SavedKeys.Level].Value as string);
            SceneManager.LoadScene(levelManager.CurrentLevel.Scene);
        }
    }
}