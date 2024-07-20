using Inventory;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Item Adder Verifier")]
    public class ItemAdderVerifierInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ItemAdderVerifier>().FromInstance(new ItemAdderVerifier(Container)).AsSingle().NonLazy();
        }
    }
}