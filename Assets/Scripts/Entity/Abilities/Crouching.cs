using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Collider2D))]
    [AddComponentMenu("Entity/Abilities/Crouching")]
    public class Crouching : Ability
    {
        public bool IsCrouching { get; set; }

        [SerializeField] private float crouchSizeMultiplier = .5f;

        private float _origCrouchSizer;
        private Collider2D _col;

        private void Start()
        {
            _col = GetComponent<Collider2D>();
        }

        public void UndoPerform()
        {
            if (!Available()) return;
            var localScale = _col.gameObject.transform.localScale;
            _col.gameObject.transform.localScale = new Vector3(localScale.x, _origCrouchSizer, localScale.z);
            IsCrouching = false;
        }

        public void Perform()
        {
            if (!Available()) return;
            var localScale = _col.gameObject.transform.localScale;
            _origCrouchSizer = localScale.y;

            /*_col.gameObject.transform.localScale = new Vector3(
                localScale.x,
                _origCrouchSizer * crouchSizeMultiplier,
                localScale.z);*/
            IsCrouching = true;
        }
    }
}