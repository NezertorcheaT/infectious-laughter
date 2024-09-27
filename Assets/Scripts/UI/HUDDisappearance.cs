using System.Collections;
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
        private bool _disappearanceNow = false;
        private float _timer;

        private void OnEnable()
        {
            if (_player.Inventory is null) return;
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
            while (true)
            {
                if (!_disappearanceNow) yield break;
                if (offsetHUD.x != 0)
                    if (disappearingHUD.position.x > _chachedHudPosition.x + offsetHUD.x - 1)
                        yield break;
                if (offsetHUD.y != 0)
                    if (disappearingHUD.position.y > _chachedHudPosition.y + offsetHUD.y - 1)
                        yield break;
                disappearingHUD.position = Vector3.Lerp(disappearingHUD.position,
                    _chachedHudPosition + new Vector3(offsetHUD.x, offsetHUD.y, 0), disappearanceSpeed);
                yield return null;
            }
        }

        private IEnumerator UnDisappearanceCycle()
        {
            _disappearanceNow = false;
            while (true)
            {
                if (_disappearanceNow) yield break;
                if (offsetHUD.x != 0)
                    if (disappearingHUD.position.x < _chachedHudPosition.x + offsetHUD.x - 1)
                        yield break;
                if (offsetHUD.y != 0)
                    if (disappearingHUD.position.y < _chachedHudPosition.y + 1)
                        yield break;
                disappearingHUD.position =
                    Vector3.Lerp(disappearingHUD.position, _chachedHudPosition, disappearanceSpeed);
                yield return null;
            }
        }
    }
}