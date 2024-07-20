using Levels.Generation;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Procedural Generation")]
    public class ProceduralGenerationInstaller : MonoInstaller
    {
        [SerializeField] private LevelRandomGeneration generator;
        [SerializeField] private ProceduralGenerationEnderInstaller ender;

        public override void InstallBindings()
        {
            ender.OnDone += InjectToSpawned;
            generator.StartGeneration();
        }

        private void InjectToSpawned()
        {
            generator.InstantiatePreSpawned(Container.InstantiatePrefab);
        }
    }
}