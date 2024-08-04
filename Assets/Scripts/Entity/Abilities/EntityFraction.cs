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
    [AddComponentMenu("Entity/Abilities/Fraction Ability")]
    public class EntityFraction : Ability
    {
#if UNITY_EDITOR
        private DropdownList<string> GetFractionTypes() => TypeCache.GetTypesDerivedFrom<Fraction>()
            .Select(i => (i.Name, i.AssemblyQualifiedName)).ToDropdownList();
#endif
        
        [Dropdown("GetFractionTypes")] [SerializeField]
        private string type;

        private Fraction _fraction;

        public Fraction Fraction
        {
            get
            {
                var t = Type.GetType(type);
                if (t is null)
                    _fraction = null;
                else
                    _fraction = Activator.CreateInstance(t) as Fraction;
                return _fraction;
            }
        }
    }
}