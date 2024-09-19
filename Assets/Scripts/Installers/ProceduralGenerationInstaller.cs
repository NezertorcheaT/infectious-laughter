using Levels.Generation;
using Levels.StoryNodes;
using Saving;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Procedural Generation")]
    public class ProceduralGenerationInstaller : MonoInstaller
    {
        public class GenerationEndSignal
        {
        }

        [SerializeField] private LevelGeneration generator;

        [Inject] private LevelManager _levelManager;
        [Inject] private SessionFactory _sessionFactory;
        [Inject] private SignalBus _signalBus;

        public override void InstallBindings()
        {
            _signalBus.DeclareSignal<GenerationEndSignal>();
            _signalBus.Subscribe<GenerationEndSignal>(InjectToSpawned);
            generator.SetSeed((string)_sessionFactory.Current[SavedKeys.Seed].Value +
                              _levelManager.LevelsPassCount +
                              _levelManager.CurrentLevel.ID);
            generator.StartGeneration();
        }

        private void InjectToSpawned()
        {
            generator.InstantiatePreSpawned(Container.InstantiatePrefab);
        }
    }
}