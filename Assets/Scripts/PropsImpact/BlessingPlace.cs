using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Installers;
using Zenject;

namespace PropsImpact
{
    public class BlessingPlace : MonoBehaviour
    {
        [SerializeField] private GameObject[] canSpawnItems;
        [Inject] private PlayerInstallation _player;
        [Inject] private Controls _actions;
        private GameObject _spawnedItem;
        private Inventory.ItemToAdd _spawnedItemToAdd;

        private void Start()
        {
            _spawnedItem = Instantiate(canSpawnItems[Random.Range(0, canSpawnItems.Length)], gameObject.transform.position, Quaternion.identity);
            _spawnedItem.SetActive(false);
            Inventory.ItemToAdd SpawnedItemToAdd = _spawnedItem.GetComponent<Inventory.ItemToAdd>();
            SpawnedItemToAdd.player = _player;
            SpawnedItemToAdd._actions = _actions;
        }

        public void UseBlessingPlace()
        {
            _spawnedItem.SetActive(true);
            Destroy(gameObject);
        }
    }
}
