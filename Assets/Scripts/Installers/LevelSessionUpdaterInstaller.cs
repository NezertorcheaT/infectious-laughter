using Levels;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Level Session Updater")]
    public class LevelSessionUpdaterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var updater = new LevelSessionUpdater();
            Container.Inject(updater);
            Container.Bind<LevelSessionUpdater>().FromInstance(updater).AsSingle().NonLazy();
        }
    }
}