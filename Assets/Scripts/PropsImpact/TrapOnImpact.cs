using UnityEngine;

namespace PropsImpact
{
    [RequireComponent(typeof(TrapImpact))]
    public class TrapOnImpact : MonoBehaviour
    {
        [SerializeField] private TrapImpact trapImpact;
        [SerializeField] private SpriteRenderer renderer1;
        [SerializeField] private SpriteRenderer renderer2;

        private void OnEnable()
        {
            trapImpact.OnTrapClosed += OnTrapClosed;
        }

        private void OnDisable()
        {
            trapImpact.OnTrapClosed -= OnTrapClosed;
        }

        private void OnTrapClosed()
        {
            renderer1.enabled = false;
            renderer2.enabled = true;
        }
    }
}