using System;
using CustomHelper;
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
            sessionFactory.Current[Helper.SavedLevelKey].Value = levelManager.CurrentLevel.ID;
            sessionFactory.SaveCurrentSession();
            saveLoader.LoadSave(sessionFactory.Current.ID);
        }

        public void EndLevelAtEnd()
        {
            OnLevelEndedAtEnd?.Invoke();

            sessionUpdater.UpdateCurrentSessionData();

            levelManager.NextLevelAtEnd();
            sessionFactory.Current[Helper.SavedLevelKey].Value = levelManager.CurrentLevel.ID;

            if (levelManager.CurrentLevel.HasShop)
            {
                SceneManager.LoadScene(levelManager.Shop);
                return;
            }

            sessionFactory.SaveCurrentSession();
            saveLoader.LoadSave(sessionFactory.Current.ID);
        }
    }
}