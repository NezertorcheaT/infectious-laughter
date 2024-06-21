using System;
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
            var config = new Config(configSaver).Deconvert(configSaver.Read(null), configSaver) as Config;
            Container.Bind<Config>().FromInstance(config).AsSingle().NonLazy();

            var sessionSaver = new SessionFileSaver();
            var sessionCreator = new SessionCreator(sessionSaver);
            Container.Bind<SessionCreator>().FromInstance(sessionCreator).AsSingle().NonLazy();

            Debug.Log(config.Volume);
            Debug.Log(SessionFileSaver.CreatePath("poo"));
            var ses = sessionCreator.NewSession();
            ses.Add(new Vector2(0, 1), "asss");
            ses.Add("amogus", "random string");
            sessionCreator.SaveCurrentSession();

            /*sessionCreator.LoadSession("0");
            foreach (var (key, content) in sessionCreator.Current)
            {
                Debug.Log(key);
                Debug.Log(content.Value);
                Debug.Log(content.Type);
            }*/
        }
    }
}