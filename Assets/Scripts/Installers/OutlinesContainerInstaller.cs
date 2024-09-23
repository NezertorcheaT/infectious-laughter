using Outline;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Outlines Container")]
    public class OutlinesContainerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var outlines = Resources.Load<OutlinesContainer>("OutlinesContainer");
            outlines.Initialize();
            Container.Bind<OutlinesContainer>().FromInstance(outlines).AsSingle().NonLazy();
        }
    }
}