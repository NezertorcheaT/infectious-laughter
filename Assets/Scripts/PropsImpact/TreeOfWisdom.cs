using Installers;
using UnityEngine;
using Zenject;

namespace PropsImpact
{
    public class TreeOfWisdom : MonoBehaviour, IUsableProp
    {
        [Inject] private PlayerInstallation _playerInstallation;
        private bool _used;
        private Entity.Abilities.Hp _hpAbility;

        private void Start()
        {
            _hpAbility = _playerInstallation.Entity.FindAbilityByType<Entity.Abilities.Hp>();
        }

        public bool TryUse()
        {
            if (_used || !isActiveAndEnabled) return false;
            Use();
            return true;
        }

        public void Use()
        {
            if (_used || !isActiveAndEnabled) return;
            _hpAbility.Health += _hpAbility.MaxHealth - _hpAbility.Health;
            _used = true;
        }
    }
}