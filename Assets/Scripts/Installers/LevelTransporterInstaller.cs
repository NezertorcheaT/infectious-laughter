using Levels;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Level Transporter")]
    public class LevelTransporterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var transporter = new LevelTransporter();
            Container.Inject(transporter);
            Container.Bind<LevelTransporter>().FromInstance(transporter).AsSingle().NonLazy();
        }
    }
}