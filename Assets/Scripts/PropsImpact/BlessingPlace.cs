using CustomHelper;
using UnityEngine;
using Installers;
using Inventory;
using Zenject;

namespace PropsImpact
{
    public class BlessingPlace : MonoBehaviour, IUsableProp
    {
        [SerializeField] private ScriptableObject[] canSpawnItems;
        [Inject] private PlayerInstallation _player;
        private Inventory.Input.PlayerInventoryInput _input;

        private void Start()
        {
            _input = _player.Entity.FindAbilityByType<Inventory.Input.PlayerInventoryInput>();
        }

        public void Use()
        {
            var item = canSpawnItems.AsType<IItem>().TakeRandom();
            if (!_input.HasSpace(item)) return;
            _input.AddItem(item.SelfRef);
            Destroy(gameObject);
        }
    }
}