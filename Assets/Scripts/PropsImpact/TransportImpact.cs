using Entity.Abilities;
using Installers;
using Levels.Generation;
using UnityEngine;
using Zenject;

namespace PropsImpact
{
    public class TransportImpact : MonoBehaviour
    {
        [Inject] private PlayerInstallation _player;
        [SerializeField] private float lifeTime = 3.0f;
        private PlayerSpawnPoint _spawnPoint;

        private void Start()
        {
            TransportPlayer();
            Destroy(gameObject, lifeTime);
        }

        private void TransportPlayer()
        {
            _spawnPoint ??= FindObjectOfType<PlayerSpawnPoint>();
            _player.Entity.FindExactAbilityByType<TransportAbility>().SetToPosition(_spawnPoint.transform.position);
        }
    }
}