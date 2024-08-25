using UnityEngine;
using Zenject;

namespace Levels
{
    public class LevelEndTrigger : MonoBehaviour
    {
        [Inject] private LevelTransporter levelTransporter;

        public void End()
        {
            levelTransporter.EndLevelAtEnd();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (
                other.GetComponent<Entity.Entity>() is null ||
                !other.gameObject.CompareTag("Player")
            ) return;
            End();
        }
    }
}