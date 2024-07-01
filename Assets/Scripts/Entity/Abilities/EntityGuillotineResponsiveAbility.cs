using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Entity/Abilities/Guillotine Responsive Ability")]
    public class EntityGuillotineResponsiveAbility : Ability
    {
        [field: SerializeField] public float NewGravityScale { get; private set; } = -0.02f;
        public Rigidbody2D Rigidbody { get; private set; }

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}