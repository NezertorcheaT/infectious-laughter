using Entity.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Inventory;
using System;

namespace PropsImpact
{
    public class SlingshotImpact : MonoBehaviour
    {
        [Inject] private Controls _actions;
        [Inject] private ItemAdderVerifier _verifier;
        [SerializeField] private GameObject rockPrefab;

        private float _startTime;
        private float _endTime;
        private readonly Vector2 _spawnOffset = new(1f, 1.6f);

        private Entity.Entity _entity;
        private float _minForce;
        private float _maxForce;
        private float _upForce;
        private float _chargeTime;
        private int _damage;
        private int _stunTime;
        private bool _initialized;

        public event Action StartCharge;
        public event Action Shot;

        public void Impact()
        {
            if (_entity == null) return;

            _actions.Gameplay.PickGarbage.canceled += EndHold;
            _startTime = Time.time;
            StartCharge?.Invoke();
        }

        public void Initialize(Entity.Entity entity, float upForce, float chargeTime, float minForce,
            float maxForce, int damage, int stunTime)
        {
            if (_initialized) return;
            _initialized = true;
            _entity = entity;
            _upForce = upForce;
            _chargeTime = chargeTime;
            _minForce = minForce;
            _maxForce = maxForce;
            _damage = damage;
            _stunTime = stunTime;
        }

        private void EndHold(InputAction.CallbackContext ctx)
        {
            _endTime = Time.time;
            _actions.Gameplay.PickGarbage.canceled -= EndHold;
            Shot?.Invoke();

            Fire(_endTime - _startTime);
            Destroy(gameObject);
        }

        private void Fire(float holdTime)
        {
            if (_entity == null) return;

            var movement = _entity.GetComponent<HorizontalMovement>();
            var position = _entity.transform.position;
            var direction = BoolToInt(movement.Turn);

            var lerpValue = Mathf.Clamp01(holdTime / _chargeTime);
            var forcePower = new Vector2(Mathf.Lerp(_minForce, _maxForce, lerpValue), 300f);

            var spawnPosition = new Vector2(
                position.x + _spawnOffset.x * direction,
                position.y + _spawnOffset.y
            );
            var rock = _verifier.Container.InstantiatePrefab(
                rockPrefab,
                spawnPosition,
                Quaternion.identity,
                null
            );

            rock.GetComponent<RockBehaviour>().Initialize(_damage, _stunTime);
            rock.GetComponent<Rigidbody2D>().AddForce(new Vector2(forcePower.x * direction, forcePower.y));

            int BoolToInt(bool value) => value ? 1 : -1;
        }
    }
}