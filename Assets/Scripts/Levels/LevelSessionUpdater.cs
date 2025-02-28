﻿using Entity.Abilities;
using Installers;
using Saving;
using Shop;
using UnityEngine;
using Zenject;

namespace Levels
{
    public class LevelSessionUpdater
    {
        [Inject] private SessionFactory sessionFactory;
        [Inject] private PlayerInstallation player;
        [Inject] private GarbageManager garbageManager;
        
        public void UpdateCurrentSessionData()
        {
            var playerHp = player.Entity.FindAbilityByType<Hp>();
            sessionFactory.Current[SavedKeys.PlayerInventory].Value = JsonUtility.ToJson(player.Inventory);
            sessionFactory.Current[SavedKeys.PlayerHp].Value = playerHp.Health - playerHp.AddictiveHealth;
            sessionFactory.Current[SavedKeys.PlayerAddictiveHp].Value = playerHp.AddictiveHealth;
            sessionFactory.Current[SavedKeys.PlayerMaxHp].Value = playerHp.MaxHealth;
            sessionFactory.Current[SavedKeys.PlayerMaxAddictiveHp].Value = playerHp.MaxAddictiveHealth;
            sessionFactory.Current[SavedKeys.PlayerGarbage].Value = garbageManager.GarbageBalance;
        }
    }
}