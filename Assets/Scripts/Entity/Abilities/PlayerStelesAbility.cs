using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/PlayerSteles Ability")]
    public class PlayerStelesAbility : Ability
    {
        EntityMovementCrouch entityMovementCrouch;

        public bool IsSteles // ���������� ����� ����� ��������� ��������� ����� � ������.
        {
            get
            {
                if (entityMovementCrouch is null) entityMovementCrouch = Entity.FindAbilityByType<EntityMovementCrouch>();
                return entityMovementCrouch.IsCrouching && Physics2D.Raycast(transform.position, Vector2.down, 1f, 1 << 6).collider != null;
            }
        }
    }
}
