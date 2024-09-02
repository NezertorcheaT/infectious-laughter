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
        [SerializeField] private ProceduralGenerationEnderInstaller proceduralGenerationEnder;
        [Inject] private SessionFactory _sessionFactory;
        [Inject] private PlayerCamera _playerCamera;

        public override void InstallBindings()
        {
            spawnPoint ??= FindObjectOfType<PlayerSpawnPoint>(true);
            var player = Container.InstantiatePrefab(
                playerPrefab,
                spawnPoint is not null ? spawnPoint.transform.position : Vector3.zero,
                Quaternion.identity, null
            ).GetComponent<Entity.Entity>();

            player.gameObject.SetActive(true);

            _playerCamera.VirtualCamera.Follow = player.transform.GetChild(1);
            _playerCamera.VirtualCamera.LookAt = player.transform;

            playerInventory ??= player.FindAbilityByType<PlayerInventoryInput>().Inventory as Inventory.Inventory;

            if (teleportCamera)
                _playerCamera.VirtualCamera.ForceCameraPosition(player.CachedTransform.position, Quaternion.identity);

            player.GetComponent<Hp>().FromContent(
                _sessionFactory.Current[SavedKeys.PlayerHp],
                _sessionFactory.Current[SavedKeys.PlayerAddictiveHp],
                _sessionFactory.Current[SavedKeys.PlayerMaxAddictiveHp],
                _sessionFactory.Current[SavedKeys.PlayerMaxHp]
            );

            JsonUtility.FromJsonOverwrite((string) _sessionFactory.Current[SavedKeys.PlayerInventory].Value,
                playerInventory);

            var pl = new PlayerInstallation(player, playerInventory);
            Container.Bind<PlayerInstallation>().FromInstance(pl).AsSingle().NonLazy();

            Container.Inject(player);

            if (spawnPoint is null)
                proceduralGenerationEnder.OnDone += () =>
                {
                    player.CachedTransform.position = FindObjectOfType<PlayerSpawnPoint>(true).transform.position;
                };
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