using Entity.Abilities;
using Installers;
using Levels.Generation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace PropsImpact
{
    public class TransportImpact : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 3.0f;
        private PlayerSpawnPoint _spawnPoint;
        [Inject] private PlayerInstallation _player;
        void Start()
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

