using Outline;
using UnityEngine;
using Zenject;

namespace Inventory.Garbage
{
    public class GarbageItem : MonoBehaviour
    {
        [SerializeField] [Min(1)] private int level;
        [SerializeField] private GameObject keyCodeTablet;
        [SerializeField] [Min(1)] private float animSpeed = 1f;
        [SerializeField] private float lifeTime = 2.5f;
        [SerializeField] private SpriteRenderer originalRenderer;
        [SerializeField] private SpriteRenderer outlineRenderer;

        [Inject] private PointTargetForGarbageAnimation _pointTargetForGarbageAnimation;
        [Inject] private OutlinesContainer _container;

        public int Level => level;

        private Transform _pointTargetUIForAnim;
        private bool _iamPicked;

        public void Suicide()
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            StartAnim();
            keyCodeTablet.SetActive(false);
        }

        private void Start()
        {
            _pointTargetUIForAnim = _pointTargetForGarbageAnimation.Target;
            outlineRenderer.sprite = _container[originalRenderer.sprite];
            outlineRenderer.enabled = false;
        }

        private void StartAnim()
        {
            _iamPicked = true;
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            if (!_iamPicked) return;
            gameObject.transform.position =
                Vector2.Lerp(
                    gameObject.transform.position,
                    _pointTargetUIForAnim.position,
                    animSpeed * Time.deltaTime /
                    Vector2.Distance(gameObject.transform.position, _pointTargetUIForAnim.position)
                );
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<Entity.Controllers.ControllerInput>()) return;
            keyCodeTablet.SetActive(other.gameObject.GetComponent<Entity.Abilities.Garbage>() is not null);
            outlineRenderer.enabled = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.GetComponent<Entity.Controllers.ControllerInput>()) return;
            keyCodeTablet.SetActive(other.gameObject.GetComponent<Entity.Abilities.Garbage>() is null);
            outlineRenderer.enabled = false;
        }
    }
}