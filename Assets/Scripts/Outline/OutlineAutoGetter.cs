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

            var newSprite = OutlinesContainer.Instance[original.sprite];
#if UNITY_EDITOR
            if (newSprite is null && OutlinesContainer.Instance.TryGenerate(original.sprite))
                newSprite = OutlinesContainer.Instance[original.sprite];
#endif
            _new.sprite = newSprite;
        }
    }
}