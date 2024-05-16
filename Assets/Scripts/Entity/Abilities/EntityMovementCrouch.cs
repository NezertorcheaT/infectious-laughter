using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Collider2D))]
    [AddComponentMenu("Entity/Abilities/Crouch Ability")]
    public class EntityMovementCrouch : Ability
    {
        public bool IsCrouching { get; set; }

        [SerializeField] private float crouchSizeMultiplier = .5f;

        private float _origCrouchSizer;
        private Collider2D _col;

        public override void Initialize()
        {
            base.Initialize();
            _col = GetComponent<Collider2D>();
        }


        public void UnCrouch()
        {
            var localScale = _col.gameObject.transform.localScale;
            _col.gameObject.transform.localScale = new Vector3(localScale.x, _origCrouchSizer, localScale.z);
            IsCrouching = false;
        }

        public void Crouch()
        {
            var localScale = _col.gameObject.transform.localScale;
            _origCrouchSizer = localScale.y;

            _col.gameObject.transform.localScale = new Vector3(
                localScale.x,
                _origCrouchSizer * crouchSizeMultiplier,
                localScale.z);
            IsCrouching = true;
        }
    }
}