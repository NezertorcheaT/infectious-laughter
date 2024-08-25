using UnityEngine;
using Zenject;

namespace GameFlow
{
    [RequireComponent(typeof(Collider2D))]
    public class CameraBounds : MonoBehaviour
    {
        [Inject] private CameraBoundsComposer composer;

        private void Start()
        {
            composer.Add(gameObject);
        }
    }
}