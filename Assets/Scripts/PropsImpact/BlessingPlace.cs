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
        private List<Inventory.ISlot> _slots;

        private void Start()
        {
            _input = _player.Entity.FindAbilityByType<Inventory.Input.PlayerInventoryInput>();
            _slots = _player.Inventory.Slots;
        }

        private bool CheckInventoryOnSpace()
        {
            for(int i = 0; i != _slots.Count; i++)
            {
                if(_slots[i].IsEmpty)
                {
                    return true;
                }
            }
            return false;
        }

        public void Use()
        {
            int i = Random.Range(0, canSpawnItems.Length);
            if(!CheckInventoryOnSpace()) return;
            _input.AddItem(canSpawnItems[i]);
            Destroy(gameObject);
        }


    }
}
