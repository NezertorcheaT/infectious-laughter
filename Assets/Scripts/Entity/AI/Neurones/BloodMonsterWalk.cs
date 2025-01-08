using System.Linq;
using Entity.Abilities;
using UnityEngine;

namespace Entity.AI.Neurones
{
    [AddComponentMenu("Entity/AI/Neurones/Blood Monster")]
    public class BloodMonsterWalk : Neurone
    {
        [SerializeField] private Eyes eyes;
        [SerializeField] private Hears hears;
        private HorizontalMovement _movement;
        private Entity _currentTarget;

        public override void AfterInitialize()
        {
            _movement = Entity.FindAvailableAbilityByInterface<HorizontalMovement>();
        }

        private void Update()
        {
            if (!Available()) return;
            if (!_movement || !_movement.Available()) return;

            _currentTarget = null;
            if (eyes && eyes.Hostiles.Count != 0)
                _currentTarget = eyes.Hostiles.LastOrDefault();
            if (hears && hears.Hostiles.Count != 0)
                _currentTarget ??= hears.Hostiles.LastOrDefault();

            if (_currentTarget)
                Debug.Log(_currentTarget);
            if (!_currentTarget) return;
        }
    }
}