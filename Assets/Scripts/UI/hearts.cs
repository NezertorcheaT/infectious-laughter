using System.Collections.Generic;
using Entity.Abilities;
using Installers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class Hearts : MonoBehaviour
    {
        [Inject] private PlayerInstallation _player;
        [SerializeField] private List<Image> lives = new();
        [SerializeField] private Sprite heartFull;
        [SerializeField] private GameObject heartPrefab;
        [SerializeField] private Transform heartsContainer;
        [SerializeField] private float spacing = 30f;
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
        
#if UNITY_EDITOR
        [Button("Test Damage Update")]
        private void DamageTest()
        {
            _currentHealth.AddDamage(2);
        }
#endif

        private void UpdateLivesList(int health, int maxHealth, int armor, int maxArmor)
        {
            foreach (var life in lives)
            {
                Destroy(life.gameObject);
            }

            lives.Clear();

            for (var i = 0; i < _currentHealth.Health; i++)
            {
                if (heartPrefab is null || heartsContainer is null) return;

                var newHeart = Instantiate(heartPrefab, heartsContainer);
                var heartImage = newHeart.GetComponent<Image>();

                if (heartImage is null) return;

                heartImage.sprite = heartFull;
                heartImage.enabled = true;

                var rectTransform = newHeart.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(i * spacing, 0);

                lives.Add(heartImage);
            }
        }
    }
}