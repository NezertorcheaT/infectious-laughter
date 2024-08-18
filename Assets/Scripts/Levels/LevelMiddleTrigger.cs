using UnityEngine;
using Zenject;

namespace Levels
{
    public class LevelMiddleTrigger : MonoBehaviour
    {
        [Inject] private LevelTransporter levelTransporter;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (
                other.GetComponent<Entity.Entity>() is null ||
                !other.gameObject.CompareTag("Player")
            ) return;
            levelTransporter.EndLevelAtMiddle();
        }
    }
}