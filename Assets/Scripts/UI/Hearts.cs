using CustomHelper;
using Entity.Abilities;
using Installers;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace UI
{
    public class Hearts : MonoBehaviour
    {
        [Inject] private PlayerInstallation _player;
        [SerializeField] private GameObject heartPrefab;
        [SerializeField] private Transform heartsContainer;
        private Hp _currentHealth;

        private void Start()
        {
            OnEnable();
            UpdateLivesList(
                _currentHealth.Health,
                _currentHealth.MaxHealth,
                _currentHealth.AddictiveHealth,
                _currentHealth.MaxAddictiveHealth
            );
        }

        private void OnEnable()
        {
            _currentHealth ??= _player.Entity.GetComponent<Hp>();
            _currentHealth.OnHpStarted += UpdateLivesList;
            _currentHealth.OnHealed += UpdateLivesList;
            _currentHealth.OnDamaged += UpdateLivesList;
        }

        private void OnDisable()
        {
            _currentHealth ??= _player.Entity.GetComponent<Hp>();
            _currentHealth.OnHpStarted -= UpdateLivesList;
            _currentHealth.OnHealed -= UpdateLivesList;
            _currentHealth.OnDamaged -= UpdateLivesList;
        }

#if UNITY_EDITOR
        [Button("Test Damage Update")]
        private void DamageTest()
        {
            _currentHealth.AddDamage(2);
        }
#endif

        private void UpdateLivesList(int health, int maxHealth, int armor, int maxArmor)
        {
            heartsContainer.ClearKids();

            for (var i = 0; i < _currentHealth.Health; i++)
            {
                if (heartPrefab is null) return;
                Instantiate(heartPrefab, heartsContainer);
            }
        }
    }
}

namespace CustomHelper
{
    public static partial class Helper
    {
        public static void ClearKids(this GameObject gameObject) => gameObject.transform.ClearKids();

        public static void ClearKids(this Transform transform)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}