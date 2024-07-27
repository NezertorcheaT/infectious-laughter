using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// кароч это энтити, используется как база для сущности<br />
    /// на неё будут навешиваться всякие хуйни типа коньтроллеров и способностей<br />
    /// коньтроллер (<c>Controller</c>) может быть только один, а вот способностей (<c>Ability</c>) сколько душе угодно
    /// </summary>
    [AddComponentMenu("Entity/Entity")]
    [DisallowMultipleComponent]
    public class Entity : MonoBehaviour
    {
        [Header("Controller")] [SerializeField]
        protected bool AutoFindController = true;

        /// <summary>
        /// эээ ну контроллер, он контролирует
        /// </summary>
        [field: SerializeField, HideIf(nameof(AutoFindController))]
        public Controller Controller { get; protected set; }

        [Header("Abilities")] [SerializeField] protected bool AutoFindAbilities = true;

        /// <summary>
        /// эта кароч массивчик способностей
        /// </summary>
        [field: SerializeField, HideIf(nameof(AutoFindAbilities))]
        public Ability[] Abilities { get; protected set; }

        /// <summary>
        /// Кэшируем все апдейты для того чтобы на каждую энтитюху вызывался только один апдейт каждого вида и мы получили парочку фпс
        /// </summary>
        public event Action OnUpdate;

        /// <summary>
        /// Кэшируем все апдейты для того чтобы на каждую энтитюху вызывался только один апдейт каждого вида и мы получили парочку фпс
        /// </summary>
        public event Action OnFixedUpdate;

        /// <summary>
        /// Кэшируем все апдейты для того чтобы на каждую энтитюху вызывался только один апдейт каждого вида и мы получили парочку фпс
        /// </summary>
        public event Action OnLateUpdate;

        /// <summary>
        /// Ахахахвахва, я кешировал трансформ! Ухахаха
        /// </summary>
        public Transform CachedTransform { get; protected set; }


        /// <summary>
        /// В идеале тут и этого быть не должно ведь класс нацелен на его наследование, ну да ладно
        /// </summary>
        protected void Awake() => Initialize();

        /// <summary>
        /// ну типа инициализация<br />
        /// нахождение контроллеров и способностей
        /// </summary>
        protected virtual void Initialize()
        {
            if (Controller == null || AutoFindController) Controller = GetComponent<Controller>();
            if (Abilities == null || AutoFindAbilities) Abilities = GetComponents<Ability>();

            CachedTransform = transform;

            foreach (var ability in Abilities)
            {
                var abil = (IInitializeByEntity) ability;
                abil.Initialized = true;
                abil.Initialize();
            }

            if (!Controller) return;
            var contr = Controller as IInitializeByEntity;
            contr.Initialized = true;
            contr.Initialize();
            Controller.OnInitializationComplete?.Invoke();
        }

        private void Update() => OnUpdate?.Invoke();
        private void FixedUpdate() => OnFixedUpdate?.Invoke();
        private void LateUpdate() => OnLateUpdate?.Invoke();

        /// <summary>
        /// ищем значт одну способность удовлетворяющую тип Т
        /// </summary>
        /// <typeparam name="T">тип способности</typeparam>
        /// <returns>способность этого типа</returns>
        public T FindAbilityByType<T>() where T : IInitializeByEntity => FindAbilitiesByType<T>().FirstOrDefault();

        /// <summary>
        /// ищем значт одну способность конкретного типа
        /// </summary>
        /// <typeparam name="T">тип способности</typeparam>
        /// <returns>способность этого типа</returns>
        public T FindExactAbilityByType<T>() where T : IInitializeByEntity =>
            FindExactAbilitiesByType<T>().FirstOrDefault();

        /// <summary>
        /// ищем значт все способности удовлетворяющие тип Т
        /// </summary>
        /// <typeparam name="T">тип способностей</typeparam>
        /// <returns>способности этого типа</returns>
        public IEnumerable<T> FindAbilitiesByType<T>() where T : IInitializeByEntity
        {
            foreach (var ability in Abilities)
            {
                if (ability is T t)
                    yield return t;
            }
        }

        /// <summary>
        /// ищем значт все способности конкретного типа
        /// </summary>
        /// <typeparam name="T">тип способностей</typeparam>
        /// <returns>способности этого типа</returns>
        public IEnumerable<T> FindExactAbilitiesByType<T>() where T : IInitializeByEntity
        {
            foreach (var ability in Abilities)
            {
                if (ability is T t && typeof(T).AssemblyQualifiedName == ability.GetType().AssemblyQualifiedName)
                    yield return t;
            }
        }

        /// <summary>
        /// ищем значт одну работающую способность по интерфейсу
        /// использовать только ПОСЛЕ OnEnable
        /// </summary>
        /// <typeparam name="T">тип способности</typeparam>
        /// <returns>способность этого типа</returns>
        public T FindAvailableAbilityByInterface<T>() where T : IInitializeByEntity =>
            FindAvailableAbilitiesByInterface<T>().FirstOrDefault();

        /// <summary>
        /// ищем значт все работающие способности по интерфейсу
        /// использовать только ПОСЛЕ OnEnable
        /// </summary>
        /// <typeparam name="T">тип способностей</typeparam>
        /// <returns>способности этого типа</returns>
        public IEnumerable<T> FindAvailableAbilitiesByInterface<T>() where T : IInitializeByEntity
        {
            foreach (var ability in Abilities)
            {
                if (ability.Available() && ability is T t) yield return t;
            }
        }
    }
}