using Inventory;
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

    private void Awake()
    {
        buttonTemplate.gameObject.SetActive(false);
    }

    public void SetItemSO(IInventory shopInventory)
    {
        foreach (ISlot slotInventory in shopInventory.Slots)
        {
            Transform slot = Instantiate(buttonTemplate, shopUITransform);
            slot.gameObject.GetComponentInChildren<Image>().sprite = slotInventory.LastItem.Sprite;
            slot.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText(slotInventory.LastItem.Name);

            slot.gameObject.GetComponentInChildren<Button>().onClick.AddListener(() => {
                OnButtonClick(slotInventory);
            });

            buttonTemplate.gameObject.SetActive(true);
        }
    }

    private void OnButtonClick(ISlot slotInventory)
    {
        //Реализация покупки предмета
        Debug.Log("Кнопка нажата");
    }

}
