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
        protected virtual void Awake() => Initialize();

        /// <summary>
        /// ну типа инициализация<br />
        /// нахождение контроллеров и способностей
        /// </summary>
        protected virtual void Initialize()
        {
            if (Controller == null || AutoFindController) Controller = GetComponent<Controller>();
            if (Abilities == null || AutoFindAbilities) Abilities = GetComponents<Ability>();

            CachedTransform = transform;

            (Controller as IInitializeByEntity)?.Initialize();
            if (Controller is IInitializeByEntity) (Controller as IInitializeByEntity).Initialized = true;
            Controller?.OnInitializationComplete?.Invoke();

            foreach (var ability in Abilities) ability?.Initialize();
        }

        private void Update() => OnUpdate?.Invoke();
        private void FixedUpdate() => OnFixedUpdate?.Invoke();
        private void LateUpdate() => OnLateUpdate?.Invoke();

        /// <summary>
        /// ищем значт одну способность конкретного типа
        /// </summary>
        /// <typeparam name="T">тип способности</typeparam>
        /// <returns>способность этого типа</returns>
        public T FindAbilityByType<T>() where T : Ability => FindAbilitiesByType<T>().FirstOrDefault();

        /// <summary>
        /// ищем значт все способности конкретного типа
        /// </summary>
        /// <typeparam name="T">тип способностей</typeparam>
        /// <returns>массивчик способностей этого типа</returns>
        public IEnumerable<T> FindAbilitiesByType<T>() where T : Ability
        {
            foreach (var ability in Abilities)
            {
                if (ability.GetType() == typeof(T)) yield return (T) ability;
            }
        }
    }
}