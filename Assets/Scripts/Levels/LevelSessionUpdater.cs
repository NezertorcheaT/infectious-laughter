using CustomHelper;
using Entity.Abilities;
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
            var playerHp = player.Entity.FindAbilityByType<EntityHp>();
            sessionFactory.Current[Helper.SavedPlayerInventoryKey].Value = JsonUtility.ToJson(player.Inventory);
            sessionFactory.Current[Helper.SavedPlayerHpKey].Value = playerHp.Hp - playerHp.AddictiveHp;
            sessionFactory.Current[Helper.SavedPlayerAddictiveHpKey].Value = playerHp.AddictiveHp;
            sessionFactory.Current[Helper.SavedPlayerMaxHpKey].Value = playerHp.MaxHp;
            sessionFactory.Current[Helper.SavedPlayerMaxAddictiveHpKey].Value = playerHp.MaxAddictiveHp;
            sessionFactory.Current[Helper.SavedPlayerGarbageKey].Value = garbageManager.GarbageBalance;
        }
    }
}