using CustomHelper;
using Entity.AI;
using UnityEngine;
using Zenject;

namespace Entity.Controllers
{
    [RequireComponent(typeof(Collider2D))]
    [AddComponentMenu("Entity/Controllers/AI Controller")]
    public class ControllerAI : Controller
    {
        [Inject] private DiContainer _container;
        [SerializeField] private Brain brain;

        public override void Initialize()
        {
            base.Initialize();
            OnInitializationComplete += OnEnable;
            OnEnable();
            if (brain is null) return;

            if (brain.IsOnPrefab())
            {
                brain = Instantiate(brain, transform);
                _container.Inject(brain);
            }

            if (brain.transform.parent != transform)
            {
                Debug.LogError(
                    $"Selected brain ({brain.gameObject.name}) is not part of de entity ({Entity.gameObject.name}). Initialization stopped");
                return;
            }

            brain.Initialize(Entity);
        }

        private void OnEnable()
        {
            if (!IsInitialized) return;
            OnInitializationComplete -= OnEnable;
        }
    }
}