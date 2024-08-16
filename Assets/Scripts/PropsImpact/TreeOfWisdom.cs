using Installers;
using UnityEngine;
using Zenject;

namespace PropsImpact
{
    public class TreeOfWisdom : MonoBehaviour
    {
        [Inject] private PlayerInstallation _playerInstallation;
        private bool _used;
        private Entity.Abilities.Hp _hpAbility;

        private void Start()
        {
            _hpAbility = _playerInstallation.Entity.FindAbilityByType<Entity.Abilities.Hp>();
        }

        public bool TryUseTreeOfWisdom()
        {
            if (_used || !isActiveAndEnabled) return false;
            UseTreeOfWisdom();
            return true;
        }

        public void UseTreeOfWisdom()
        {
            if (_used || !isActiveAndEnabled) return;
            _hpAbility.Heal(_hpAbility.MaxHealth - _hpAbility.Health);
            _used = true;
        }
    }
}