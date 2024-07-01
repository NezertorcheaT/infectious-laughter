using Cinemachine;
using Inventory;
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
        private ItemAdderVerifier _adderVerifier;

        public override void InstallBindings()
        {
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