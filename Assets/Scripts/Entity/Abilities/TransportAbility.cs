using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Transport")]
    public class TransportAbility : Ability
    {
        public void SetToPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}