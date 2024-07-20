using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Procedural Generation End")]
    public class ProceduralGenerationEnderInstaller : MonoInstaller
    {
        public event Action OnDone;

        public override void InstallBindings()
        {
            OnDone?.Invoke();
        }
    }
}