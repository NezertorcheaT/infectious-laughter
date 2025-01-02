using Entity.Abilities;
using UnityEngine;

namespace Entity.AI.Neurones
{
    [AddComponentMenu("Entity/AI/Neurones/Basic Jumper")]
    public class BasicJumper : Neurone
    {
        [SerializeField] private BasicEye eye;
        private IJumpableAbility _jumpableAbility;

        public override void AfterInitialize()
        {
            _jumpableAbility = Entity.FindAvailableAbilityByInterface<IJumpableAbility>();
        }

        private void Update()
        {
            if (eye.IsSeeing) _jumpableAbility.Perform();
        }
    }
}