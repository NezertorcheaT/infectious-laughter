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
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material outlineMaterial;

        [Inject] private PointTargetForGarbageAnimation _pointTargetForGarbageAnimation;

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
            if(!other.GetComponent<Entity.Controllers.ControllerInput>()) return;
            keyCodeTablet.SetActive(other.gameObject.GetComponent<Entity.Abilities.EntityGarbage>() is not null);
            gameObject.GetComponent<SpriteRenderer>().material = outlineMaterial;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(!other.GetComponent<Entity.Controllers.ControllerInput>()) return;
            keyCodeTablet.SetActive(other.gameObject.GetComponent<Entity.Abilities.EntityGarbage>() is null);
            gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
        }
    }
}