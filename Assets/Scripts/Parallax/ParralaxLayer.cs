using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ParralaxLayer : MonoBehaviour
{
    [SerializeField] private Vector2 parralaxMultiplier;
    [Inject] private Camera _camera;

    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;

    private void Start()
    {
        _cameraTransform = _camera.transform;
        _lastCameraPosition = _camera.transform.position;
    }
    private void LateUpdate()
    {
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
        gameObject.transform.position += new Vector3(deltaMovement.x * parralaxMultiplier.x, deltaMovement.y * parralaxMultiplier.y, 0f);
        _lastCameraPosition = _cameraTransform.position;
    }
}