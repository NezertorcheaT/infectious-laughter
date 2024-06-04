using UnityEngine;
using System.Collections;

namespace Inventory.Garbage
{
    public class GarbageItem : MonoBehaviour
    {
        [Min(1)] public int Level;
        [Space(10f)] [SerializeField] private GameObject _keyCodeTablet;
        [SerializeField] private GameObject _pointTargetUIForAnim;
        [SerializeField] private Sprite _ballAnimSprite;
        [SerializeField] private bool _iamPicked;
        [SerializeField, Min(1)] private float _animSpeed;
        private float LifeTime = 2.5f;
        [SerializeField] private GameObject I; // Ссылка на самого себя
        [SerializeField] private Material DefaultMaterial;
        [SerializeField] private Material OutlineMaterial;    

        public void Suicide()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = _ballAnimSprite;
            gameObject.GetComponent<Collider2D>().enabled = false;
            //_iamPicked = true;
            StartCoroutine(StartAnim(LifeTime));
            _keyCodeTablet.SetActive(false);
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 10,gameObject.transform.localScale.y * 10,gameObject.transform.localScale.x * 10);
            //Destroy(gameObject);
        }
        private IEnumerator StartAnim(float DeathDelay)
        {
            _iamPicked = true;
            yield return new WaitForSeconds(DeathDelay);
            Destroy(I);
        }
        private void Update()
        {
            if(_iamPicked)
            {
                gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, _pointTargetUIForAnim.transform.position, _animSpeed * Time.deltaTime * Vector2.Distance(gameObject.transform.position, _pointTargetUIForAnim.transform.position));
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _keyCodeTablet.SetActive(other.gameObject.GetComponent<Entity.Abilities.EntityGarbage>());
            gameObject.GetComponent<SpriteRenderer>().material = OutlineMaterial;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _keyCodeTablet.SetActive(!other.gameObject.GetComponent<Entity.Abilities.EntityGarbage>());
            gameObject.GetComponent<SpriteRenderer>().material = DefaultMaterial;
        }
    }
}