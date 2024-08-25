using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class ItemFrame : MonoBehaviour
    {
        [field: SerializeField] public Image Item { get; private set; }
    }
}