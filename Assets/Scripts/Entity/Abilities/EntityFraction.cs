using System;
using System.Reflection;
using Entity.Relationships;
using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Fraction Ability")]
    public class EntityFraction : Ability
    {
        [SerializeField, HideInInspector] private string type;
        private Fraction _fraction;

        public Fraction Fraction
        {
            get
            {
                var t = Type.GetType(type);
                if (t is null) _fraction = null;
                GetType().GetField("_fraction", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    ?.SetValue(this, Activator.CreateInstance(t));
                return _fraction;
            }
        }
    }
}