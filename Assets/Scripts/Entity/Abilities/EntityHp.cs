using System;
using Saving;
using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/HP Ability")]
    public class EntityHp : Ability
    {
        [SerializeField] private int health = 5;
        [SerializeField] private int addictiveHealth;
        [SerializeField] private int maxAddictiveHealth;
        [SerializeField] private int maxHealth = 5;

        public Action<int, int, int, int> OnDamaged;
        public Action<int, int, int, int> OnHealed;
        public Action<int, int, int, int> OnHpStarted;
        public Action<int, int> OnAddictiveHpChanged;
        public Action OnDie;

        public int AddictiveHp
        {
            get => addictiveHealth;
            set
            {
                if (addictiveHealth == value) return;

                addictiveHealth = Mathf.Clamp(value, 0, maxAddictiveHealth);
                OnAddictiveHpChanged?.Invoke(addictiveHealth, maxAddictiveHealth);
            }
        }
        public int Hp
        {
            get => health + AddictiveHp;

            private set
            {
                if (Hp < value)
                {
                    health = Mathf.Min(maxHealth, value);
                    OnHealed?.Invoke(health, AddictiveHp, maxAddictiveHealth, maxHealth);
                    return;
                }

                if (Hp > value)
                {
                    value -= Hp;
                    var addictiveHpNow = AddictiveHp + value;
                    if (addictiveHpNow < 0)
                        health = Mathf.Clamp(health + addictiveHpNow, 0, maxHealth);
                    else
                        AddictiveHp = Mathf.Max(0, addictiveHpNow);
                    OnDamaged?.Invoke(health, AddictiveHp, maxAddictiveHealth, maxHealth);
                }
            }
        }

        public int MaxAddictiveHp => maxAddictiveHealth;
        public int MaxHp => maxHealth;

        public void AddDamage(int d)
        {
            Hp -= Mathf.Max(d, 0);

            if (Hp <= 0) OnDie?.Invoke();
        }

        public void Heal(int d)
        {
            d = Mathf.Max(d, 0);
            Hp += d;
        }

        public void FromContent(
            Session.Content hpContent,
            Session.Content addictiveHpContent,
            Session.Content maxAddictiveHealthContent,
            Session.Content maxHealthContent
        )
        {
            health = (int) hpContent.Value;
            addictiveHealth = (int) addictiveHpContent.Value;
            maxHealth = (int) maxHealthContent.Value;
            maxAddictiveHealth = (int) maxAddictiveHealthContent.Value;
        }
    }
}