using Saving;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Save System")]
    public class SaveSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var configSaver = new ConfigFileSaver();
            var config = new Config(configSaver);
            Container.Bind<Config>().FromInstance(config).AsSingle().NonLazy();

            var sessionSaver = new SessionFileSaver();
            var sessionCreator = new SessionCreator(sessionSaver);
            Container.Bind<SessionCreator>().FromInstance(sessionCreator).AsSingle().NonLazy();
        }
    }
}