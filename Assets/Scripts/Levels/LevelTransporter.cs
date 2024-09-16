using System;
using GameFlow;
using Levels.StoryNodes;
using Saving;
using UnityEngine.SceneManagement;
using Zenject;

namespace Levels
{
    public class LevelTransporter
    {
        [Inject] private LevelManager levelManager;
        [Inject] private SessionFactory sessionFactory;
        [Inject] private MenuSaveLoader saveLoader;
        [Inject] private LevelSessionUpdater sessionUpdater;

        public event Action OnLevelEndedAtEnd;
        public event Action OnLevelEndedAtMiddle;

        public void EndLevelAtMiddle()
        {
            if (!levelManager.HasNextLevelAtMiddle()) return;

            OnLevelEndedAtMiddle?.Invoke();

            sessionUpdater.UpdateCurrentSessionData();

            levelManager.NextLevelAtMiddle();
            sessionFactory.Current[SavedKeys.Level].Value = levelManager.CurrentLevel.ID;
            sessionFactory.SaveCurrentSession();
            saveLoader.LoadSave(sessionFactory.Current.ID);
        }

        public void EndLevelAtEnd()
        {
            OnLevelEndedAtEnd?.Invoke();

            sessionUpdater.UpdateCurrentSessionData();

            levelManager.NextLevelAtEnd();
            sessionFactory.Current[SavedKeys.Level].Value = levelManager.CurrentLevel.ID;
            sessionFactory.SaveCurrentSession();

            if (levelManager.CurrentLevel.HasShop)
            {
                levelManager.SetLevel(sessionFactory.Current[SavedKeys.Level].Value as string);
                SceneManager.LoadScene(levelManager.Shop);
                return;
            }

            saveLoader.LoadSave(sessionFactory.Current.ID);
        }
    }
}