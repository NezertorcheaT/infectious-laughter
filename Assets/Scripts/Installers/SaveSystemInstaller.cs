using System.IO;
using System.Linq;
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
            string configText;

            try
            {
                configText = configSaver.Read(null);
            }
            catch (FileNotFoundException)
            {
                configSaver.Save(config);
                configText = configSaver.Read(null);
            }

            config = config.Deconvert(configText, configSaver) as Config;
            Container.Bind<Config>().FromInstance(config).AsSingle().NonLazy();

            var sessionSaver = new SessionFileSaver();
            var sessionCreator = new SessionFactory(sessionSaver);
            Container.Bind<SessionFactory>().FromInstance(sessionCreator).AsSingle().NonLazy();
#if UNITY_EDITOR
            //для запуска любых сцен в инспекторе
            if (!sessionCreator.GetAvailableSessionIDs().Any())
            {
                Debug.LogError("Вам нужно иметь хотя бы одно сохранение, нажмите Играть в сцене NewGameTest один раз");
                return;
            }

            sessionCreator.LoadSession(sessionCreator.GetAvailableSessionIDs().First());
#endif
        }
    }
}