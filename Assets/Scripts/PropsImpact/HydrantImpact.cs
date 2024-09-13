using System;
using System.Threading.Tasks;
using Entity.Abilities;
using UnityEngine;

namespace PropsImpact
{
    public class HydrantImpact : MonoBehaviour
    {
        [SerializeField] private Sprite defaultHydrant;
        [SerializeField] private Sprite activatedHydrant;
        [SerializeField] private Sprite deactivatedHydrant;
        [SerializeField] private float elevateTimeInSeconds = 2.5f;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material outlineMaterial;
        private SpriteRenderer _spriteRenderer;
        private BuoyancyEffector2D _effector;
        private bool _wasActivated;

        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _effector = GetComponent<BuoyancyEffector2D>();
            _spriteRenderer.sprite = defaultHydrant;
            _wasActivated = false;
        }

        public void StartElevating()
        {
            if (!_wasActivated)
            {
                _wasActivated = true;
                _spriteRenderer.sprite = activatedHydrant;
                var elevateTimeInMiliseconds = (int)(elevateTimeInSeconds * 1000);
                Elevate(elevateTimeInMiliseconds);
                _effector.enabled = true;
            }
        }

        private async void Elevate(int time)
        {
            await Task.Delay(time);
            _spriteRenderer.sprite = deactivatedHydrant;
            _effector.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<Entity.Controllers.ControllerInput>() || _wasActivated) return;
            gameObject.GetComponent<SpriteRenderer>().material = outlineMaterial;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.GetComponent<Entity.Controllers.ControllerInput>() || !_wasActivated) return;
            gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
        }
    }
}
