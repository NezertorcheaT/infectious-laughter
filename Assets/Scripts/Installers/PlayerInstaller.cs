using Entity.Abilities;
using GameFlow;
using Inventory;
using Inventory.Input;
using Levels.Generation;
using Saving;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Player")]
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Entity.Entity playerPrefab;
        [SerializeField] private Inventory.Inventory playerInventory;
        [SerializeField] private bool teleportCamera;
        [SerializeField] private PlayerSpawnPoint spawnPoint;
        [Inject] private SessionFactory _sessionFactory;
        [Inject] private PlayerCamera _playerCamera;
        [Inject] private SignalBus _signalBus;
        private Entity.Entity _player;

        public override void InstallBindings()
        {
            spawnPoint ??= FindObjectOfType<PlayerSpawnPoint>(true);
            _player = Container.InstantiatePrefab(
                playerPrefab,
                spawnPoint is not null ? spawnPoint.transform.position : Vector3.zero,
                Quaternion.identity, null
            ).GetComponent<Entity.Entity>();

            _player.gameObject.SetActive(true);

            _playerCamera.VirtualCamera.Follow = _player.transform.GetChild(1);
            _playerCamera.VirtualCamera.LookAt = _player.transform;

            playerInventory ??= _player.FindAbilityByType<PlayerInventoryInput>().Inventory as Inventory.Inventory;

            if (teleportCamera)
                _playerCamera.VirtualCamera.ForceCameraPosition(_player.CachedTransform.position, Quaternion.identity);

            _player.GetComponent<Hp>().FromContent(
                _sessionFactory.Current[SavedKeys.PlayerHp],
                _sessionFactory.Current[SavedKeys.PlayerAddictiveHp],
                _sessionFactory.Current[SavedKeys.PlayerMaxAddictiveHp],
                _sessionFactory.Current[SavedKeys.PlayerMaxHp]
            );

            JsonUtility.FromJsonOverwrite((string)_sessionFactory.Current[SavedKeys.PlayerInventory].Value,
                playerInventory);

            var pl = new PlayerInstallation(_player, playerInventory);
            Container.Bind<PlayerInstallation>().FromInstance(pl).AsSingle().NonLazy();

            Container.Inject(_player);

            if (spawnPoint is null && _signalBus.IsSignalDeclared<ProceduralGenerationInstaller.GenerationEndSignal>())
                _signalBus.Subscribe<ProceduralGenerationInstaller.GenerationEndSignal>(OnGenerationEnded);
        }

        private void OnGenerationEnded()
        {
            _player.CachedTransform.position = FindObjectOfType<PlayerSpawnPoint>(true).transform.position;
            _signalBus.Unsubscribe<ProceduralGenerationInstaller.GenerationEndSignal>(OnGenerationEnded);
        }
    }

    public readonly struct PlayerInstallation
    {
        public Entity.Entity Entity { get; }
        public IInventory Inventory { get; }

        public PlayerInstallation(Entity.Entity player, IInventory inventory)
        {
            Entity = player;
            Inventory = inventory;
        }
    }
}