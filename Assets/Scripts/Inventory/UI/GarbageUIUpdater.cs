using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GarbageUIUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _garbageText;
    public void UpdateGarbageUI() => _garbageText.text = gameObject.GetComponent<Entity.Abilities.EntityGarbage>().GarbageBalance.ToString();
}
