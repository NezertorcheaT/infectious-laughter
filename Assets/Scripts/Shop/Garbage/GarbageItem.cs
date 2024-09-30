using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Outline;
using UnityEngine;
using Zenject;

namespace Shop.Garbage
{
    public class GarbageItem : MonoBehaviour
    {
        [SerializeField] [Min(1)] private int level;
        [SerializeField] private GameObject keyCodeTablet;
        [SerializeField] private GameObject trailPrefab;
        [SerializeField] [Min(0)] private float animSpeed = 1f;
        [SerializeField] private SpriteRenderer originalRenderer;
        [SerializeField] private SpriteRenderer outlineRenderer;

        [Inject] private PointTargetForGarbageAnimation _pointTargetForGarbageAnimation;

        public int Level => level;

        private Transform _pointTargetUIForAnim;
        private bool _iamPicked;

        public void Suicide()
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            keyCodeTablet.SetActive(false);
            originalRenderer.enabled = false;
            outlineRenderer.enabled = false;
            StartAnim();
        }

        private void Start()
        {
            _pointTargetUIForAnim = _pointTargetForGarbageAnimation.Target;
            outlineRenderer.sprite = OutlinesContainer.ToOutline(originalRenderer.sprite);
            outlineRenderer.enabled = false;
        }

        private async Task StartAnim()
        {
            if (_iamPicked) return;
            _iamPicked = true;

            var trail = Instantiate(trailPrefab, transform.position, Quaternion.identity, null);
            for (var i = 0f; i < animSpeed; i += Time.fixedDeltaTime)
            {
                await UniTask.WaitForFixedUpdate();
                trail.transform.position =
                    Vector3.Slerp(transform.position, _pointTargetUIForAnim.position, i / animSpeed);
            }

            await UniTask.WaitForFixedUpdate();

            Destroy(trail);
            Destroy(transform.parent.gameObject);
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