using System;
using Entity;
using Installers;
using UnityEngine;
using Zenject;

namespace Inventory.Input
{
    [AddComponentMenu("Entity/Abilities/Inventory Picking Ability")]
    public class PlayerInventoryInput : Ability, IInventoryInput
    {
        [Inject] private ItemAdderVerifier _itemAdderVerifier;
        [SerializeField] private ScriptableObject inventory;

        //Сделали его пабликом для того что бы можно было вызывать у боссов. Или у скилов.
        public float MaxDistance = 5f;

        public IInventory Inventory => inventory as IInventory;

        private void Start()
        {
            if (Inventory.Empty) return;
            foreach (var slot in Inventory.Slots)
            {
                if (!(slot.LastItem is ICanSpawn i)) continue;
                i.Verifier = _itemAdderVerifier;
            }
        }

        public void AddItem(ScriptableObject item)
        {
            if (!(item is IItem inventoryItem)) return;

            if (inventoryItem is ICanSpawn i)
            {
                i.Verifier = _itemAdderVerifier;
                inventoryItem = i;
            }

            if (Available())
                Inventory?.TryAddItem(inventoryItem);
        }

        public bool HasSpace(IItem item)
        {
            return ((IInventory) inventory).TryAddItem(item, true, false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, MaxDistance);
        }
    }
}