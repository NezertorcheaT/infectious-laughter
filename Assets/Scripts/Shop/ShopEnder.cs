using GameFlow;
using Saving;
using UnityEngine;
using Zenject;

namespace Shop
{
    public class ShopEnder : MonoBehaviour
    {
        [Inject] private SessionFactory sessionFactory;
        [Inject] private MenuSaveLoader saveLoader;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Entity.Entity>() is null) return;
            sessionFactory.SaveCurrentSession();
            saveLoader.LoadSave(sessionFactory.Current.ID);
        }
    }
}