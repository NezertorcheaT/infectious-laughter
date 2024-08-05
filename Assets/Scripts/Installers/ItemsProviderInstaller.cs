using Inventory;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Items Provider")]
    public class ItemsProviderInstaller : MonoInstaller
    {
        [SerializeField] private ItemsProvider provider;

        public override void InstallBindings()
        {
            provider.Initialize();
        }
    }
}