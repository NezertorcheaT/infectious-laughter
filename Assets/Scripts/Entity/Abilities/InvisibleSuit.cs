using System.Linq;
using System.Collections;
using CustomHelper;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Invisible Suit")]
    [RequireComponent(typeof(Fraction))]
    public class InvisibleSuit : Ability
    {
#if UNITY_EDITOR
        private DropdownList<string> GetFractionTypes() => TypeCache.GetTypesDerivedFrom<Relationships.Fraction>()
            .Select(i => (i.Name, i.AssemblyQualifiedName)).ToDropdownList();
#endif

        [SerializeField] private int workTime;

        [Dropdown("GetFractionTypes")] [SerializeField]
        private string _friendlyForMonstersType;

        private string _chachedTypePlayerFraction;

        private Fraction _playerFraction;

        public void UseSuit()
        {
            if (!Available()) return;
            StartCoroutine(UseSuitCoroutine());
        }

        private void Start()
        {
            _playerFraction = gameObject.GetComponent<Fraction>();
            _chachedTypePlayerFraction = _playerFraction.type;
        }

        private IEnumerator UseSuitCoroutine()
        {
            _playerFraction.type = _friendlyForMonstersType;
            yield return new WaitForSeconds(workTime);
            _playerFraction.type = _chachedTypePlayerFraction;
        }
    }
}