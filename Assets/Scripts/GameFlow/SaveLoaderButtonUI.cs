using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFlow
{
    public class SaveLoaderButtonUI : MonoBehaviour
    {
        [field: SerializeField] public Button Enter { get; private set; }
        [field: SerializeField] public Button Remove { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TextMesh { get; private set; }
    }
}