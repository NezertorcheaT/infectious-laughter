using Cinemachine;
using CustomHelper;
using Entity.Abilities;
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
        [Inject] private CinemachineVirtualCamera _playerCinemachineCamera;

        public override void InstallBindings()
        {
            spawnPoint ??= FindObjectOfType<PlayerSpawnPoint>(true);
            var player = Container.InstantiatePrefab(
                playerPrefab,
                spawnPoint is not null ? spawnPoint.transform.position : Vector3.zero,
                Quaternion.identity, null
            ).GetComponent<Entity.Entity>();

            player.gameObject.SetActive(true);

            _playerCinemachineCamera.Follow = player.transform.GetChild(1);
            _playerCinemachineCamera.LookAt = player.transform;

            playerInventory ??= player.FindAbilityByType<PlayerInventoryInput>().Inventory as Inventory.Inventory;

            if (teleportCamera)
                _playerCinemachineCamera.ForceCameraPosition(player.CachedTransform.position, Quaternion.identity);

            player.GetComponent<EntityHp>().FromContent(
                _sessionFactory.Current[SavedKeys.PlayerHp],
                _sessionFactory.Current[SavedKeys.PlayerAddictiveHp],
                _sessionFactory.Current[SavedKeys.PlayerMaxHp],
                _sessionFactory.Current[SavedKeys.PlayerMaxAddictiveHp]
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