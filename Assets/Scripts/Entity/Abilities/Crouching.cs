using PropsImpact;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Collider2D))]
    [AddComponentMenu("Entity/Abilities/Crouching")]
    public class Crouching : Ability
    {
        public bool IsCrouching { get; private set; }

        [SerializeField] private float crouchSizeMultiplier = 0.5f;
        [SerializeField, Min(0)] private float crouchSpeedMultiplier = 0.5f;

        private float _origCrouchSizer;
        private Collider2D _col;
        private HorizontalMovement _movement;

        private void Start()
        {
            _col = GetComponent<Collider2D>();
            _movement = GetComponent<HorizontalMovement>();
        }

        public void UndoPerform()
        {
            if (!Available() || !IsCrouching) return;
            var localScale = _col.gameObject.transform.localScale;
            _col.gameObject.transform.localScale = new Vector3(localScale.x, _origCrouchSizer, localScale.z);
            IsCrouching = false;
            if (_movement) _movement.Speed /= crouchSpeedMultiplier;
        }

        public void Perform()
        {
            if (!Available() || IsCrouching) return;
            var localScale = _col.gameObject.transform.localScale;
            _origCrouchSizer = localScale.y;
            if (_movement) _movement.Speed *= crouchSpeedMultiplier;

            /*_col.gameObject.transform.localScale = new Vector3(
                localScale.x,
                _origCrouchSizer * crouchSizeMultiplier,
                localScale.z);*/
            IsCrouching = true;
        }
    }
}