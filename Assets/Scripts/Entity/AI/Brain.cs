using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace Entity.AI
{
    /// <summary>
    /// вот это мозг<br />
    /// он должен быть отдельным гейобжектом, под сущностью<br />
    /// также может являться префабом
    /// </summary>
    [AddComponentMenu("Entity/AI/Brain")]
    [DisallowMultipleComponent]
    public class Brain : MonoBehaviour
    {
        [SerializeField] private bool autoFindNeurones = true;

        [SerializeField, HideIf(nameof(autoFindNeurones))]
        private List<Neurone> neurones;

        private bool _initialized;

        /// <summary>
        /// нейроны мозга
        /// </summary>
        public IEnumerable<Neurone> Neurones;

        /// <summary>
        /// внутренний метод для инициализации нейронов
        /// </summary>
        /// <param name="entity"></param>
        public void Initialize([NotNull] Entity entity)
        {
            if (_initialized) return;
            if (autoFindNeurones)
            {
                neurones = new List<Neurone>(10);
                neurones.AddRange(gameObject.GetComponents<Neurone>());
            }

            foreach (var neurone in neurones)
                neurone.Initialize(entity, this);
            foreach (var neurone in neurones)
                neurone.AfterInitialize();
            _initialized = true;
        }
    }
}