using System;
using Saving;
using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/HP")]
    public class Hp : Ability
    {
        [SerializeField] private int health = 3;
        [SerializeField] private int addictiveHealth;
        [SerializeField] private int maxAddictiveHealth;
        [SerializeField] private int maxHealth = 3;

        public event Action<int, int, int, int> OnDamaged;
        public event Action<int, int, int, int> OnHealed;
        public event Action<int, int, int, int> OnHpStarted;
        public event Action<int, int> OnAddictiveHpChanged;
        public event Action OnDie;
        public event Action BeforeDie;

        public int AddictiveHealth
        {
            get => addictiveHealth;
            set
            {
                value = Mathf.Max(value, 0);
                if (addictiveHealth == value) return;

                addictiveHealth = Mathf.Clamp(value, 0, maxAddictiveHealth);
                OnAddictiveHpChanged?.Invoke(addictiveHealth, maxAddictiveHealth);
            }
        }

        public int Health
        {
            get => health + AddictiveHealth;
            set
            {
                value = Mathf.Max(value, 0);
                if (Health < value)
                {
                    health = Mathf.Min(maxHealth, value);
                    OnHealed?.Invoke(health, AddictiveHealth, maxAddictiveHealth, maxHealth);
                    return;
                }

                if (Health > value)
                {
                    value -= Health;
                    var addictiveHpNow = AddictiveHealth + value;
                    if (addictiveHpNow < 0)
                        health = Mathf.Clamp(health + addictiveHpNow, 0, maxHealth);
                    else
                        AddictiveHealth = Mathf.Max(0, addictiveHpNow);
                    OnDamaged?.Invoke(health, AddictiveHealth, maxAddictiveHealth, maxHealth);
                    if (Health > 0) return;
                    BeforeDie?.Invoke();
                    if (Health > 0) return;
                    OnDie?.Invoke();
                }
            }
        }

        public int MaxAddictiveHealth => maxAddictiveHealth;
        public int MaxHealth => maxHealth;

        public void FromContent(
            Session.Content hpContent,
            Session.Content addictiveHpContent,
            Session.Content maxAddictiveHealthContent,
            Session.Content maxHealthContent
        )
        {
            health = (int)hpContent.Value;
            addictiveHealth = (int)addictiveHpContent.Value;
            maxHealth = (int)maxHealthContent.Value;
            maxAddictiveHealth = (int)maxAddictiveHealthContent.Value;
        }
    }
}