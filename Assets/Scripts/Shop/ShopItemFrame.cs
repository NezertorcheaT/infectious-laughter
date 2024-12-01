using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopItemFrame : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
        [field: SerializeField] public Image Item { get; private set; }
        [field: SerializeField] public Button Button { get; private set; }
        [SerializeField] private Animator animator;
        public bool Brought { get; private set; }

        public void OnBye()
        {
            Brought = true;
        }

        public void Animate() => animator?.Play("start");
        public void Stop() => animator?.Play("pre");
    }
}