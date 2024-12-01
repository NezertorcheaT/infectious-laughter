using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using Inventory;
using UnityEngine;

namespace Shop
{
    public class ShopGenerator : MonoBehaviour
    {
        [SerializeField, Min(1)] private int maxCapacity;
        [SerializeField] private ScriptableObject[] shopItemsPoolList;
        [SerializeField] private ShopUI shopUi;

        private void Awake()
        {
            var items = shopItemsPoolList.AsType<IShopItem>().ToArray();
            var gallery = new List<IShopItem>(maxCapacity);

            for (var i = 0; i < maxCapacity; i++)
            {
                gallery.Add(items[Random.Range(0, items.Length)]);
            }

            shopUi.SetShopWindow(gallery);
        }
    }
}