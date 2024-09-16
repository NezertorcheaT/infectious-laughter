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

        public void End()
        {
            sessionUpdater.UpdateCurrentSessionData();
            sessionFactory.SaveCurrentSession();
            saveLoader.LoadSave(sessionFactory.Current.ID);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Entity.Controllers.ControllerInput>() is null) return;
            End();
        }
    }
}