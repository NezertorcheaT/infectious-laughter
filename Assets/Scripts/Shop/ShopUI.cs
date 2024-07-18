using Entity.Abilities;
using Inventory;
using Inventory.Input;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform buttonTemplate;
    [SerializeField] private Transform shopUITransform;
    [SerializeField] private EntityGarbage garbage;


    private Transform[] slotsArray;
    private void Awake()
    {
        buttonTemplate.gameObject.SetActive(false);
    }

    public void SetItemSO(IInventory shopInventory)
    {
        slotsArray = new Transform[shopInventory.Slots.Count];

        foreach (ISlot slotInventory in shopInventory.Slots)
        {
            Transform slot = Instantiate(buttonTemplate, shopUITransform);
            slot.gameObject.GetComponentInChildren<Image>().sprite = slotInventory.LastItem.Sprite;
            slot.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText(slotInventory.LastItem.ItemCost.ToString());

            slot.gameObject.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                OnButtonClick(slot, slotInventory);
            });

            buttonTemplate.gameObject.SetActive(true);

        }
    }

    private void OnButtonClick(Transform slot, ISlot slotInventory)
    {
        //Реализация покупки предмета
        if (!GarbageManager.Instance.IfCanAfford(slotInventory.LastItem.ItemCost) || slotInventory.Count <= 0) { return; }
        if (!PlayerInventoryInput.Instance.HasSpace(slotInventory.LastItem)) { return; }

        GarbageManager.Instance.SpentGarbageBalance(slotInventory.LastItem.ItemCost);
        slotInventory.Count = 0;

        PlayerInventoryInput.Instance.AddItem(slotInventory.LastItem.SelfRef);

        slot.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("SOLD");
        
    }


}
