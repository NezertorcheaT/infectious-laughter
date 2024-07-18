using GameFlow;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Saves Loader")]
    public class SaveLoaderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var saveLoader = new MenuSaveLoader();
            Container.Inject(saveLoader);
            Container.Bind<MenuSaveLoader>().FromInstance(saveLoader).AsSingle();
        }
    }
}