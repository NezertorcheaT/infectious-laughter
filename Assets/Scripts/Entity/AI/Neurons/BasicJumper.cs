using System.Linq;
using Entity.Abilities;
using UnityEngine;

namespace Entity.AI.Neurons
{
    [AddComponentMenu("Entity/AI/Neurones/Basic Jumper")]
    public class BasicJumper : Neurone
    {
        [SerializeField] private Hears hears;
        private IJumpableAbility _jumpableAbility;

        public override void AfterInitialize()
        {
            _jumpableAbility = Entity.FindAvailableAbilityByInterface<IJumpableAbility>();
        }

        private void Update()
        {
            if (hears.Available() && hears.Hostiles.Any()) _jumpableAbility.Perform();
        }
    }
}