﻿using System;
using Entity;
using Installers;
using UnityEngine;
using Zenject;

namespace Inventory.Input
{
    [AddComponentMenu("Entity/Abilities/Inventory Picking Ability")]
    public class PlayerInventoryInput : Ability, IInventoryInput
    {
        [Inject] private PlayerInstallation _player;
        [SerializeField] private ScriptableObject inventory;
        [SerializeField] private float maxDistance = 5f;

        public IInventory Inventory => inventory as IInventory;
        public float MaxDistance => maxDistance;

        private void Start()
        {
            if(Inventory.Empty) return;
            foreach (var slot in Inventory.Slots)
            {
                if (!(slot.LastItem is ICanSpawn i)) continue;
                i.Verifier = _player.ItemAdderVerifier;
            }
        }

        public void AddItem(ScriptableObject item)
        {
            if (!(item is IItem inventoryItem)) return;

            if (inventoryItem is ICanSpawn i)
            {
                i.Verifier = _player.ItemAdderVerifier;
                inventoryItem = i;
            }

            if (Available())
                Inventory?.TryAddItem(inventoryItem);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, maxDistance);
        }
    }
}