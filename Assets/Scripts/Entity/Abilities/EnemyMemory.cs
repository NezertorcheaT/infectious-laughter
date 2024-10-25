using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Enemy Memory")]
    public class EnemyMemory : Ability
    {
        public Vector2 StartPosition;
        public Vector2 LastPlayerPosition;
        public Entity FollowEnemy;
    }
}