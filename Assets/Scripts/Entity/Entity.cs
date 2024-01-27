using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Entity
{
    public class Entity : MonoBehaviour
    {
        [Header("Controller")] [SerializeField]
        protected bool AutoFindController = true;

        [field: SerializeField, HideIf(nameof(AutoFindController))]
        public Controller Controller { get; protected set; }

        [Header("Abilities")] [SerializeField] protected bool AutoFindAbilities = true;

        [field: SerializeField, HideIf(nameof(AutoFindAbilities))]
        public Ability[] Abilities { get; protected set; }

        // Кэшируем все апдейты для того чтобы на каждую энтитюху вызывался только один апдейт каждого вида и мы получили парочку фпс
        public event Action OnUpdate;
        public event Action OnFixedUpdate;
        public event Action OnLateUpdate;
        

        /// <summary>
        /// В идеале тут и этого быть не должно ведь класс нацелен на его наследование, ну да ладно
        /// </summary>
        protected virtual void Awake() => Initialize();

        protected virtual void Initialize()
        {
            if (Controller == null || AutoFindController) Controller = GetComponent<Controller>();
            if (Abilities == null || AutoFindAbilities) Abilities = GetComponents<Ability>();

            Controller?.Initialize();
            foreach (var ability in Abilities) ability?.Initialize();
        }

        private void Update() => OnUpdate?.Invoke();
        private void FixedUpdate() => OnFixedUpdate?.Invoke();
        private void LateUpdate() => OnLateUpdate?.Invoke();

        public T FindAbilityByType<T>() where T : Ability => FindAbilitiesByType<T>().FirstOrDefault();

        public IEnumerable<T> FindAbilitiesByType<T>() where T : Ability
        {
            foreach (var ability in Abilities)
            {
                if (ability.GetType() == typeof(T)) yield return (T) ability;
            }
        }
    }
}