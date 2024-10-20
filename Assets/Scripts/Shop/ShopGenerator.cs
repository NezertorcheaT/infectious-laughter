using System.Collections.Generic;
using System.Linq;
using Inventory;
using UnityEngine;

namespace Shop
{
    public class ShopGenerator : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObject> shopItemsPoolList;
        [SerializeField] private ScriptableObject inventoryShop;
        [SerializeField] private ShopUI shopUi;

        private IInventory _inventory;
        private IItem[] _items;

        private void Awake()
        {
            _inventory = inventoryShop as IInventory;
            if (_inventory is null) return;
            _inventory.ClearInventory();

            _items = shopItemsPoolList.Select(item => item as IItem).Where(item => item is not null).ToArray();

            GenerateItemsInShop(_inventory.MaxCapacity);
        }

        private void GenerateItemsInShop(int itemsCountToGenerate)
        {
            if (_inventory is null) return;

            for (var i = 0; i <= itemsCountToGenerate; i++)
            {
                if (_items[Random.Range(0, _items.Length)] is not IShopItem item)
                {
                    i--;
                    continue;
                }

                _inventory.TryAddItem(item, false);
            }

            shopUi.SetShopwindow(_inventory);
        }
    }
}