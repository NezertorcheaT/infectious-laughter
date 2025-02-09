using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/SoundPlayer")]
    public class SoundPlayerInstaller : MonoInstaller
    {
        [SerializeField] private SoundPlayer player;
        public override void InstallBindings()
        {
            var playerInstance = Container.InstantiatePrefabForComponent<SoundPlayer>(player);

            Container.Bind<SoundPlayer>().FromInstance(playerInstance).AsSingle().NonLazy();
        }
    }
}