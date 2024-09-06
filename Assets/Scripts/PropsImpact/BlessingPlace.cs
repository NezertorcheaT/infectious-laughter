using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Installers;
using Zenject;

namespace PropsImpact
{
    public class BlessingPlace : MonoBehaviour, IUsableProp
    {
        [SerializeField] private ScriptableObject[] canSpawnItems;
        [Inject] private PlayerInstallation _player;
        private Inventory.Input.PlayerInventoryInput _input;

        private void Start()
        {
            _input = _player.Entity.FindAbilityByType<Inventory.Input.PlayerInventoryInput>();
        }

        public void Use()
        {
            int i = Random.Range(0, canSpawnItems.Length);
            if(!_input.HasSpace((Inventory.IItem)canSpawnItems[i])) return;
            _input.AddItem(canSpawnItems[i]);
            Destroy(gameObject);
        }


    }
}
