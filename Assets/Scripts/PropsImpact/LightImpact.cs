using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LightImpact : MonoBehaviour
{
    public bool InLight { get; private set; }

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float maxShakingForce;
    [SerializeField] private float shakingProgressionRate = 1;

    private CinemachineBasicMultiChannelPerlin _cinemachineShaker;
    private float _shakingForce = 0;
    private Entity.Abilities.EntityMovementHorizontalMove _movementComp;
    private float _chachedSpeed;

    private void Start()
    {
        _cinemachineShaker = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Entity.Controllers.ControllerInput>()) return;
        _movementComp = other.GetComponent<Entity.Abilities.EntityMovementHorizontalMove>();
        InLight = true;
        _chachedSpeed = _movementComp.Speed;
        _movementComp.Speed = _chachedSpeed / 2;
        StartCoroutine(ShakeCamera());
        StopCoroutine(ToDefaultShakeCamera());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.GetComponent<Entity.Controllers.ControllerInput>()) return;
        InLight = false;
        _movementComp.Speed = _chachedSpeed;
        StartCoroutine(ToDefaultShakeCamera());
        StopCoroutine(ShakeCamera());
    }

    private IEnumerator ToDefaultShakeCamera()
    {
        while(!InLight && _shakingForce > 0)
        {
            _shakingForce -= 1;
            _cinemachineShaker.m_AmplitudeGain = _shakingForce;
            yield return new WaitForSeconds(shakingProgressionRate);
        }
    }

    private IEnumerator ShakeCamera()
    {
        while(_shakingForce < maxShakingForce && InLight)
        {
            _shakingForce += 1;
            _cinemachineShaker.m_AmplitudeGain = _shakingForce;
            yield return new WaitForSeconds(shakingProgressionRate);
        }
    }
}
