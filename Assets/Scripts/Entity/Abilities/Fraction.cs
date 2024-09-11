using System;
using System.Linq;
using System.Reflection;
using CustomHelper;
using Entity.Relationships;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Fraction")]
    public class Fraction : Ability
    {
#if UNITY_EDITOR
        private DropdownList<string> GetFractionTypes() => TypeCache.GetTypesDerivedFrom<Relationships.Fraction>()
            .Select(i => (i.Name, i.AssemblyQualifiedName)).ToDropdownList();
#endif
        
        [Dropdown("GetFractionTypes")] [SerializeField]
        public string type;

        private Relationships.Fraction _fraction;

        public Relationships.Fraction CurrentFraction
        {
            get
            {
                var t = Type.GetType(type);
                if (t is null)
                    _fraction = null;
                else
                    _fraction = Activator.CreateInstance(t) as Relationships.Fraction;
                return _fraction;
            }
        }
    }
}