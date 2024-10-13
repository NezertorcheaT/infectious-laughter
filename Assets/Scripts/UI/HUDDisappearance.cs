using System.Collections;
using System.Threading;
using CustomHelper;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Installers;

namespace UI
{
    public class HUDDisappearance : MonoBehaviour
    {
        [SerializeField] private float disappearanceSpeed;
        [SerializeField] private float timeToDisappearance;
        [SerializeField] private Vector2 offsetHUD;
        [SerializeField] private Transform disappearingHUD;
        [Inject] private PlayerInstallation _player;
        [Inject] private Controls _actions;
        private Vector3 _chachedHudPosition;
        private Vector3 _newHudPosition;
        private bool _disappearanceNow = false;
        private float _timer;

        private void OnEnable()
        {
            if (_player.Inventory is null) return;
            _actions.Gameplay.Dash.performed += TimerToZero;
            _actions.Gameplay.AltClick.performed += TimerToZero;
            _actions.Gameplay.Move.performed += TimerToZero;
            _actions.Gameplay.Jump.performed += TimerToZero;
            _actions.Gameplay.Run.performed += TimerToZero;
            _actions.Gameplay.PickGarbage.performed += TimerToZero;
            _actions.Gameplay.MouseWheel.performed += TimerToZero;
            _actions.Gameplay.Inv_selectSlot1.performed += TimerToZero;
            _actions.Gameplay.Inv_selectSlot2.performed += TimerToZero;
            _actions.Gameplay.Inv_selectSlot3.performed += TimerToZero;
            _actions.Gameplay.Inv_selectSlot4.performed += TimerToZero;
            _actions.Gameplay.Inv_selectSlot5.performed += TimerToZero;
            _actions.Gameplay.Inv_selectSlot6.performed += TimerToZero;
        }

        private void OnDisable()
        {
            if (_player.Inventory is null) return;
            _actions.Gameplay.Dash.performed -= TimerToZero;
            _actions.Gameplay.AltClick.performed -= TimerToZero;
            _actions.Gameplay.Move.performed -= TimerToZero;
            _actions.Gameplay.Jump.performed -= TimerToZero;
            _actions.Gameplay.Run.performed -= TimerToZero;
            _actions.Gameplay.PickItem.performed -= TimerToZero;
            _actions.Gameplay.PickGarbage.performed -= TimerToZero;
            _actions.Gameplay.MouseWheel.performed -= TimerToZero;
            _actions.Gameplay.Inv_selectSlot1.performed -= TimerToZero;
            _actions.Gameplay.Inv_selectSlot2.performed -= TimerToZero;
            _actions.Gameplay.Inv_selectSlot3.performed -= TimerToZero;
            _actions.Gameplay.Inv_selectSlot4.performed -= TimerToZero;
            _actions.Gameplay.Inv_selectSlot5.performed -= TimerToZero;
            _actions.Gameplay.Inv_selectSlot6.performed -= TimerToZero;
        }

        private void TimerToZero(InputAction.CallbackContext ctx) => TimerZero();

        private void TimerZero()
        {
            _timer = 0;
        }

        private void Update()
        {
            if (_timer > timeToDisappearance)
            {
                if (!_disappearanceNow) Disappearance();
                return;
            }

            _timer += Time.deltaTime;
            if (_disappearanceNow) UnDisappearance();
        }

        private void Start()
        {
            _chachedHudPosition = disappearingHUD.position;
            _newHudPosition = _chachedHudPosition + offsetHUD.ToVector3();
        }

        private void Disappearance()
        {
            StartCoroutine(DisappearanceCycle());
        }

        private void UnDisappearance()
        {
            StartCoroutine(UnDisappearanceCycle());
        }

        private IEnumerator DisappearanceCycle()
        {
            _disappearanceNow = true;
            for (var i = 0f; i < disappearanceSpeed; i += Time.fixedDeltaTime)
            {
                if (!_disappearanceNow) yield break;
                yield return new WaitForFixedUpdate();
                disappearingHUD.position = Vector3.Lerp(_chachedHudPosition, _newHudPosition, i / disappearanceSpeed);
            }
        }

        private IEnumerator UnDisappearanceCycle()
        {
            _disappearanceNow = false;
            for (var i = 0f; i < disappearanceSpeed; i += Time.fixedDeltaTime)
            {
                if (_disappearanceNow) yield break;
                yield return new WaitForFixedUpdate();
                disappearingHUD.position = Vector3.Lerp(_newHudPosition, _chachedHudPosition, i / disappearanceSpeed);
            }
        }
    }
}