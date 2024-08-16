using GameFlow;
using Levels;
using Saving;
using UnityEngine;
using Zenject;

namespace Shop
{
    public class ShopLevelEnder : MonoBehaviour
    {
        [Inject] private LevelSessionUpdater sessionUpdater;
        [Inject] private SessionFactory sessionFactory;
        [Inject] private MenuSaveLoader saveLoader;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Entity.Entity>() is null) return;
            sessionUpdater.UpdateCurrentSessionData();
            sessionFactory.SaveCurrentSession();
            saveLoader.LoadSave(sessionFactory.Current.ID);
        }
    }
}