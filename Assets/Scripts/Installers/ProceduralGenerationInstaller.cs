using CustomHelper;
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
        [SerializeField] private LevelRandomGeneration generator;
        [SerializeField] private ProceduralGenerationEnderInstaller ender;

        [Inject] private LevelManager _levelManager;
        [Inject] private SessionFactory _sessionFactory;

        public override void InstallBindings()
        {
            ender.OnDone += InjectToSpawned;
            generator.seed = (_sessionFactory.Current.SaveCorruptCheck()
                                 ? (string) _sessionFactory.Current[SavedKeys.Seed].Value
                                 : "пени") +
                             _levelManager.LevelsPassCount +
                             _levelManager.CurrentLevel.ID;
            generator.StartGeneration();
        }

        private void InjectToSpawned()
        {
            generator.InstantiatePreSpawned(Container.InstantiatePrefab);
        }
    }
}