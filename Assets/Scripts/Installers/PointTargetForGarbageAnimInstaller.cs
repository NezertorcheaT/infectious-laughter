using Inventory.Garbage;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Target for Garbage Animation")]
    public class PointTargetForGarbageAnimInstaller : MonoInstaller
    {
        [SerializeField] private Transform pointTargetUIForAnim;

        public override void InstallBindings()
        {
            var target = new PointTargetForGarbageAnimation(pointTargetUIForAnim);
            Container.Bind<PointTargetForGarbageAnimation>().FromInstance(target).AsSingle().NonLazy();
        }
    }
}