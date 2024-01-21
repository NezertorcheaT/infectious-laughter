using System;
using UnityEngine;
using NaughtyAttributes;

namespace Scripts.Entity
{
    public class Entity : MonoBehaviour
    {
        [Header("Controller")]
        [SerializeField] protected bool AutoFindController = true;
        [field: SerializeField, HideIf(nameof(AutoFindController))] public Controller Controller { get; protected set; }

        [Header("Abilities")]
        [SerializeField] protected bool AutoFindAbilities = true;
        [field: SerializeField, HideIf(nameof(AutoFindAbilities))] public Ability[] Abilities { get; protected set; }

        public event Action OnUpdate;       // 
        public event Action OnFixedUpdate;  // Кэшируем все апдейты для того чтобы на каждую энтитюху вызывался только один апдейт каждого вида и мы получили парочку фпс
        public event Action OnLateUpdate;   // 

        protected virtual void Awake() => Initialize(); // В идеале тут и этого быть не должно ведь класс нацелен на его наследование, ну да ладно
        protected virtual void Initialize()
        {
            if (Controller == null || AutoFindController) Controller = GetComponent<Controller>();
            if (Abilities == null || AutoFindAbilities) Abilities = GetComponents<Ability>();

            Controller.Initialize();
            foreach (Ability ability in Abilities) ability.Initialize();
        }
        private void Update() => OnUpdate?.Invoke();
        private void FixedUpdate() => OnFixedUpdate?.Invoke();
        private void LateUpdate() => OnLateUpdate?.Invoke();

        public T FindAbilityByType<T>() where T : Ability
        {
            foreach (Ability ability in Abilities)
            {
                if (ability.GetType() == typeof(T)) return (T)ability;
            }
            return null;
        }
    }
}