using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Player Steles Ability")]
    public class PlayerStelesAbility : Ability
    {
        EntityMovementCrouch entityMovementCrouch;

        public bool IsSteles // ���������� ����� ����� ��������� ��������� ����� � ������.
        {
            get
            {
                entityMovementCrouch ??= Entity.FindAbilityByType<EntityMovementCrouch>();
                return entityMovementCrouch.IsCrouching && Physics2D.Raycast(transform.position, Vector2.down, 1f, 1 << 6).collider != null;
            }
        }
    }
}
