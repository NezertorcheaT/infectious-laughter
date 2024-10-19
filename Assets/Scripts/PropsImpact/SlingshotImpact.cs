using Entity.Abilities;
using Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Inventory;
using static UnityEngine.EventSystems.EventTrigger;
using System;


namespace PropsImpact
{
    public class SlingshotImpact : MonoBehaviour
    {
        [Inject] private Controls _actions;
        [Inject] public ItemAdderVerifier Verifier { get; set; }
        [SerializeField] private GameObject RockPrefab;

        private float startTime;
        private float endTime;
        private Vector2 spawnOffset = new Vector2(1f, 1.6f);

        private Entity.Entity entity;       
        private float minForce;
        private float maxForce;
        private float upForce;
        private float chargeTime;
        private int damage;
        private int stunTime;

        public event Action StartCharge;
        public event Action Shot;

        public void Impact()
        {
            if (entity == null) return;

            _actions.Gameplay.PickGarbage.canceled += EndHold;
            startTime = Time.time;
            StartCharge?.Invoke();
        }

        public void Initialize(Entity.Entity _entity, float _upForce, float _chargeTime, float _minForce, float _maxForce, int _damage, int _stunTime)
        {
            entity = _entity;
            upForce = _upForce;
            chargeTime = _chargeTime;
            minForce = _minForce;
            maxForce = _maxForce;
            damage = _damage;
            stunTime = _stunTime;
        }

        private void EndHold(InputAction.CallbackContext ctx)
        {
            endTime = Time.time;
            float holdTime = endTime - startTime;
            //Debug.Log(holdTime);
            _actions.Gameplay.PickGarbage.canceled -= EndHold;
            Shot?.Invoke();

            Fire(holdTime);

            Destroy(gameObject);
        }

        private void Fire(float holdTime)
        {
            if (entity == null) return;

            var movment = entity.GetComponent<HorizontalMovement>();
            var position = entity.transform.position;
            var direction = BoolToInt(movment.Turn);

            float lerpValue = Mathf.Clamp01(holdTime / chargeTime);
            var forcePower = new Vector2(Mathf.Lerp(minForce, maxForce, lerpValue), 300f);

            var spawnPosition =
                new Vector2(position.x + spawnOffset.x * direction, position.y + spawnOffset.y);
            var rock =
                Verifier.Container.InstantiatePrefab(RockPrefab, spawnPosition, Quaternion.identity, null);

            rock.GetComponent<RockBehaviour>().Initialize(damage, stunTime);
            rock.GetComponent<Rigidbody2D>().AddForce(new Vector2(forcePower.x * direction, forcePower.y));

            int BoolToInt(bool value) => value ? 1 : -1;
        }
    }
}
