using System.Threading.Tasks;
using Outline;
using UnityEngine;

namespace PropsImpact
{
    public class HydrantImpact : MonoBehaviour
    {
        [SerializeField] private Sprite defaultHydrant;
        [SerializeField] private Sprite activatedHydrant;
        [SerializeField] private Sprite deactivatedHydrant;
        [SerializeField] private float elevateTimeInSeconds = 2.5f;
        [SerializeField] private SpriteRenderer originalRenderer;
        [SerializeField] private SpriteRenderer outlineRenderer;
        private BuoyancyEffector2D _effector;
        private bool _wasActivated;

        private void Start()
        {
            originalRenderer ??= GetComponent<SpriteRenderer>();
            _effector = GetComponent<BuoyancyEffector2D>();
            originalRenderer.sprite = defaultHydrant;
            _wasActivated = false;
            outlineRenderer.sprite = OutlinesContainer.ToOutline(originalRenderer.sprite);
        }

        public void StartElevating()
        {
            if (!_wasActivated)
            {
                _wasActivated = true;
                originalRenderer.sprite = activatedHydrant;
                var elevateTimeInMiliseconds = (int)(elevateTimeInSeconds * 1000);
                Elevate(elevateTimeInMiliseconds);
                _effector.enabled = true;
            }
        }

        private async void Elevate(int time)
        {
            await Task.Delay(time);
            originalRenderer.sprite = deactivatedHydrant;
            _effector.enabled = false;
        }
    }
}