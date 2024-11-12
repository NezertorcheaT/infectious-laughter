using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainCanvasInstaller : MonoInstaller
    {

        [SerializeField] private GameObject _mainCanvas;


        public override void InstallBindings()
        {
            var ci = new MainCanvasInstallation(_mainCanvas);
            Container.Bind<MainCanvasInstallation>().FromInstance(ci).AsSingle().NonLazy();

        }

        public readonly struct MainCanvasInstallation
        {
            public GameObject CanvasObject { get; }

            public MainCanvasInstallation(GameObject canvas)
            {
                CanvasObject = canvas;
            }
        }
    }
}