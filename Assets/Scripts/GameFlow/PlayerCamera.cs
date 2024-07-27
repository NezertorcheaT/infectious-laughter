using Cinemachine;
using UnityEngine;

namespace GameFlow
{
    public class PlayerCamera : MonoBehaviour
    {
        [field: SerializeField] public Transform PointTargetForGarbageAnimation { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera VirtualCamera { get; private set; }
        [field: SerializeField] public Camera MainCamera { get; private set; }
    }
}