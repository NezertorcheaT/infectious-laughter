using GameFlow;
using Shop.Garbage;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Target for Garbage Animation")]
    public class PointTargetForGarbageAnimInstaller : MonoInstaller
    {
        [Inject] private PlayerCamera cam;

        public override void InstallBindings()
        {
            var target = new PointTargetForGarbageAnimation(cam.PointTargetForGarbageAnimation);
            Container.Bind<PointTargetForGarbageAnimation>().FromInstance(target).AsSingle().NonLazy();
        }
    }
}