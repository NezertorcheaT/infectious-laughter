using CustomHelper;
using Saving;
using Shop;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Garbage Manager")]
    public class GarbageManagerInstaller : MonoInstaller
    {
        [Inject] private SessionFactory sessionFactory;

        public override void InstallBindings()
        {
            var garbageManager = new GarbageManager((int)sessionFactory.Current[SavedKeys.PlayerGarbage].Value);
            Container.Bind<GarbageManager>().FromInstance(garbageManager).AsSingle();
        }
    }
}