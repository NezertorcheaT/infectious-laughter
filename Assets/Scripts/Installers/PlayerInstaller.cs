using Cinemachine;
using CustomHelper;
using Entity.Abilities;
using Inventory;
using Saving;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Player")]
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Entity.Entity player;
        [SerializeField] private Inventory.Inventory playerInventory;
        [SerializeField] private CinemachineVirtualCamera playerCinemachineCamera;
        [Inject] private SessionFactory sessionFactory;
        private ItemAdderVerifier _adderVerifier;

        public override void InstallBindings()
        {
            player.GetComponent<EntityHp>().FromContent(
                sessionFactory.Current[Helper.SavedPlayerHpKey],
                sessionFactory.Current[Helper.SavedPlayerAddictiveHpKey],
                sessionFactory.Current[Helper.SavedPlayerMaxHpKey],
                sessionFactory.Current[Helper.SavedPlayerMaxAddictiveHpKey]
            );

            JsonUtility.FromJsonOverwrite((string) sessionFactory.Current[Helper.SavedPlayerInventoryKey].Value, playerInventory);

            _adderVerifier = new ItemAdderVerifier(Container);
            var pl = new PlayerInstallation(player, playerInventory, _adderVerifier, playerCinemachineCamera);
            Container.Bind<PlayerInstallation>().FromInstance(pl).AsSingle().NonLazy();
        }
    }

    public readonly struct PlayerInstallation
    {
        public Entity.Entity Entity { get; }
        public IInventory Inventory { get; }
        public ItemAdderVerifier ItemAdderVerifier { get; }
        public CinemachineVirtualCamera ViewCamera { get; }

        public PlayerInstallation(Entity.Entity player, IInventory inventory, ItemAdderVerifier adderVerifier,
            CinemachineVirtualCamera viewCamera)
        {
            Entity = player;
            Inventory = inventory;
            ItemAdderVerifier = adderVerifier;
            ViewCamera = viewCamera;
        }
    }
}