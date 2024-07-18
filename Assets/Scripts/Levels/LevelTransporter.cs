using System;
using Entity.Abilities;
using GameFlow;
using Installers;
using Levels.StoryNodes;
using Saving;
using Shop;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Levels
{
    public class LevelTransporter
    {
        [Inject] private LevelManager levelManager;
        [Inject] private SessionFactory sessionFactory;
        [Inject] private MenuSaveLoader saveLoader;
        [Inject] private PlayerInstallation player;
        [Inject] private GarbageManager garbageManager;

        public event Action OnLevelEndedAtEnd;
        public event Action OnLevelEndedAtMiddle;

        public void EndLevelAtMiddle()
        {
            if (!levelManager.HasNextLevelAtMiddle()) return;

            OnLevelEndedAtMiddle?.Invoke();
            levelManager.NextLevelAtMiddle();
            sessionFactory.Current[NewGameStarter.SavedLevelKey].Value = levelManager.CurrentLevel.ID;
            sessionFactory.SaveCurrentSession();
            saveLoader.LoadSave(sessionFactory.Current.ID);
        }

        public void EndLevelAtEnd()
        {
            OnLevelEndedAtEnd?.Invoke();
            var playerHp = player.Entity.FindAbilityByType<EntityHp>();

            sessionFactory.Current[NewGameStarter.SavedPlayerInventoryKey].Value = JsonUtility.ToJson(player.Inventory);
            sessionFactory.Current[NewGameStarter.SavedPlayerHpKey].Value = playerHp.Hp - playerHp.AddictiveHp;
            sessionFactory.Current[NewGameStarter.SavedPlayerAddictiveHpKey].Value = playerHp.AddictiveHp;
            sessionFactory.Current[NewGameStarter.SavedPlayerMaxHpKey].Value = playerHp.MaxHp;
            sessionFactory.Current[NewGameStarter.SavedPlayerMaxAddictiveHpKey].Value = playerHp.MaxAddictiveHp;
            sessionFactory.Current[NewGameStarter.SavedPlayerGarbageKey].Value = garbageManager.GarbageBalance;

            levelManager.NextLevelAtEnd();
            sessionFactory.Current[NewGameStarter.SavedLevelKey].Value = levelManager.CurrentLevel.ID;

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