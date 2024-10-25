using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Pin")]
    public class PinAbility : Ability
    {
        [SerializeField] private Transform PinPoint;

        private GameObject _pinnedObject;

        private void Awake()
        {
            enabled = false;
        }

        private void FixedUpdate()
        {
            _pinnedObject.transform.position = PinPoint.position;
        }

        public void Initialize(GameObject pinnedObject)
        {
            _pinnedObject = pinnedObject;
            StartPin();
        }

        private void StartPin()
        {
            enabled = true;
        }

        public void StopPin()
        {
            enabled = false;
        }
    }
}