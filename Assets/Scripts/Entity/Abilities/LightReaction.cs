using System.Collections;
using Cinemachine;
using GameFlow;
using PropsImpact;
using UnityEngine;
using Zenject;

namespace Entity.Abilities
{
    [RequireComponent(typeof(LightResponsive))]
    [RequireComponent(typeof(HorizontalMovement))]
    [AddComponentMenu("Entity/Abilities/Light Reaction")]
    public class LightReaction : Ability
    {
        [SerializeField] private float maxShakingForce;
        [SerializeField] private float shakingProgressionRate = 1;
        [SerializeField] private float speedMultiplier = 0.5f;

        [Inject] private PlayerCamera _camera;
        private LightResponsive _responsive;
        private HorizontalMovement _movement;

        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private CinemachineBasicMultiChannelPerlin _cinemachineShaker;

        private float _shakingForce;
        private float _storedSpeed;

        private void Start()
        {
            _responsive = Entity.FindAvailableAbilityByInterface<LightResponsive>();
            _movement = Entity.FindAvailableAbilityByInterface<HorizontalMovement>();

            _cinemachineVirtualCamera = _camera.VirtualCamera;
            _cinemachineShaker =
                _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            OnEnable();
        }

        private void OnEnable()
        {
            if (!IsInitialized || _responsive is null) return;
            _responsive.OnEnterLight += OnEnterLight;
            _responsive.OnExitLight += OnExitLight;
        }

        private void OnDisable()
        {
            _responsive.OnEnterLight -= OnEnterLight;
            _responsive.OnExitLight -= OnExitLight;
            StopAllCoroutines();
        }

        private void OnEnterLight(LightImpact impact)
        {
            _storedSpeed = _movement.Speed;
            _movement.Speed = _storedSpeed * speedMultiplier;
            StartCoroutine(ShakeCamera());
            StopCoroutine(ToDefaultShakeCamera());
        }

        private void OnExitLight(LightImpact impact)
        {
            _movement.Speed = _storedSpeed;
            StartCoroutine(ToDefaultShakeCamera());
            StopCoroutine(ShakeCamera());
        }

        private IEnumerator ToDefaultShakeCamera()
        {
            while (!_responsive.InLight && _shakingForce > 0)
            {
                _shakingForce -= 1;
                _cinemachineShaker.m_AmplitudeGain = _shakingForce;
                yield return new WaitForSeconds(shakingProgressionRate);
            }
        }

        private IEnumerator ShakeCamera()
        {
            while (_shakingForce < maxShakingForce && _responsive.InLight)
            {
                _shakingForce += 1;
                _cinemachineShaker.m_AmplitudeGain = _shakingForce;
                yield return new WaitForSeconds(shakingProgressionRate);
            }
        }
    }
}