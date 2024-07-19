using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/PlayerSteles Ability")]
    public class PlayerStelesAbility : Ability
    {
        EntityMovementCrouch entityMovementCrouch;

        public bool IsSteles // Вызывается когда нужно проверить находится игрок в стелсе.
        {
            get
            {
                if (entityMovementCrouch is null) entityMovementCrouch = Entity.FindAbilityByType<EntityMovementCrouch>();

                // Выполняем физическую проверку на наличие объекта под игроком
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, 1 << 6);

                return entityMovementCrouch.IsCrouching && hit.collider != null;
            }
        }
    }
}
