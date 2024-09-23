using UnityEngine;

namespace Outline
{
    [AddComponentMenu("Effects/Outline")]
    [RequireComponent(typeof(SpriteRenderer))]
    public class OutlineAutoGetter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer original;
        private SpriteRenderer _new;

        private void Awake()
        {
            _new ??= GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            UpdateOutline();
        }

        public void UpdateOutline()
        {
            _new ??= GetComponent<SpriteRenderer>();
            if (original.sprite is null)
            {
                _new.sprite = null;
                return;
            }

            _new.sprite = OutlinesContainer.Instance[original.sprite];
        }
    }
}