using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Camera Bounds")]
    public class CameraBoundsInstaller : MonoInstaller
    {
        [SerializeField] private Transform collectionPrefab;

        public override void InstallBindings()
        {
            var collection = Container.InstantiatePrefab(collectionPrefab).transform;
            var composer = new CameraBoundsComposer(collection);
            Container.Bind<CameraBoundsComposer>().FromInstance(composer).AsSingle();
        }
    }
}

public class CameraBoundsComposer
{
    private Transform _parent;
    public CompositeCollider2D Collider { get; }
    public event Action OnAdded;

    public CameraBoundsComposer(Transform parent)
    {
        _parent = parent;
        Collider = _parent.GetComponent<CompositeCollider2D>();
    }

    public void Add(GameObject instancedGameObject)
    {
        if (!instancedGameObject.activeInHierarchy) return;
        var colliders = instancedGameObject.GetComponentsInChildren<Collider2D>();
        if (colliders.Length == 0) return;
        foreach (var collider in colliders)
        {
            collider.usedByComposite = true;
        }

        instancedGameObject.transform.SetParent(_parent, true);
        OnAdded?.Invoke();
    }
}