using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Stealth")]
    public class Stealth : Ability
    {
        private Crouching _movementCrouch;

        public bool IsStealth // Вызывается когда нужно проверить находится игрок в стелсе.
        {
            get
            {
                _movementCrouch ??= Entity.FindAbilityByType<Crouching>();
                return _movementCrouch.IsCrouching && Physics2D.Raycast(transform.position, Vector2.down, 1f, 1 << 6).collider != null;
            }
        }
    }
}
