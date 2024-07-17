using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGenerator : MonoBehaviour
{
    [SerializeField] private List<ScriptableObject> shopItemsPoolList;
    [SerializeField] private ScriptableObject inventoryShop;
    [SerializeField] private Transform shopUi;

    private void Awake()
    {
        ((IInventory)inventoryShop).ClearInventory();
        GenerateItemsInShop(((IInventory)inventoryShop).MaxCapacity);
    }

    private void GenerateItemsInShop(int itemsCountToGenerate)
    {
        IInventory shopInventory = (IInventory)inventoryShop;

        for (int i = 0; i <= itemsCountToGenerate; i++)
        {
            shopInventory.TryAddItem((IItem)shopItemsPoolList[Random.Range(0, shopItemsPoolList.Count)]);
        }

        //Вызыв магазин
        shopUi.GetComponent<ShopUI>().SetItemSO(shopInventory);
    }
}
