using System;
using Installers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

namespace GameFlow
{
    public class PopUp : MonoBehaviour
    {
        public event Action OnUsed;
        public event Action OnEntered;
        public event Action OnExited;
        
        [SerializeField] private GameObject popUp;
        [SerializeField] private UnityEvent onUsed;
        [SerializeField] private UnityEvent onEntered;
        [SerializeField] private UnityEvent onExited;
        [Inject] private PlayerInstallation _player;
        [Inject] private Controls _controls;
        private bool _inTrigger;

        private void OnEnable()
        {
            if (_controls is null) return;
            _controls.Gameplay.PickGarbage.performed += Use;
        }

        private void OnDisable()
        {
            if (_controls is null) return;
            _controls.Gameplay.PickGarbage.performed -= Use;
        }

        private void Start()
        {
            OnEnable();
        }

        private void Use(InputAction.CallbackContext context)
        {
            if (!isActiveAndEnabled || !_inTrigger) return;
            popUp.SetActive(false);
            onUsed.Invoke();
            OnUsed?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var entity = other.GetComponent<Entity.Entity>();
            if (entity is null || entity != _player.Entity) return;
            _inTrigger = true;
            popUp.SetActive(_inTrigger);
            onEntered.Invoke();
            OnEntered?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var entity = other.GetComponent<Entity.Entity>();
            if (entity is null || entity != _player.Entity) return;
            _inTrigger = false;
            popUp.SetActive(_inTrigger);
            onExited.Invoke();
            OnExited?.Invoke();
        }
    }
}