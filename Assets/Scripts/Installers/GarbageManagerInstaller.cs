using Shop;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Garbage Manager")]
    public class GarbageManagerInstaller : MonoInstaller
    {
        [SerializeField] private int baseGarbageAmount = 50;

        public override void InstallBindings()
        {
            var manage = new GarbageManager(baseGarbageAmount);
            Container.Bind<GarbageManager>().FromInstance(manage).AsSingle();
        }
    }
}