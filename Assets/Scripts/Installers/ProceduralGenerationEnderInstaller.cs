using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Procedural Generation End")]
    public class ProceduralGenerationEnderInstaller : MonoInstaller
    {
        [Inject] private SignalBus _signalBus;

        public override void InstallBindings()
        {
            if (_signalBus.IsSignalDeclared<ProceduralGenerationInstaller.GenerationEndSignal>())
                _signalBus.Fire<ProceduralGenerationInstaller.GenerationEndSignal>();
        }
    }
}