using Levels.StoryNodes;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Level Manager")]
    public class LevelManagerInstaller : MonoInstaller
    {
        [SerializeField] private StoryTree tree;
        [SerializeField] private int shopScene;

        public override void InstallBindings()
        {
            var levelManager = new LevelManager(tree, shopScene);
            Container.Bind<LevelManager>().FromInstance(levelManager).AsSingle().NonLazy();
        }
    }
}