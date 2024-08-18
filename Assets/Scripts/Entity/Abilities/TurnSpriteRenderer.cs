using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(HorizontalMovement))]
    [AddComponentMenu("Entity/Abilities/Turn Sprite Renderer")]
    public class TurnSpriteRenderer : Ability
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField, Min(0)] private int delay = 100;
        private HorizontalMovement _movement;

        private void Start()
        {
            OnEnable();
            FragmentedUpdate();
        }

        private void OnEnable()
        {
            if (Entity)
                _movement ??= Entity.FindExactAbilityByType<HorizontalMovement>();
            if (_movement)
                _movement.OnTurn += OnTurn;
        }

        private void OnDisable()
        {
            if (Entity)
                _movement ??= Entity.FindExactAbilityByType<HorizontalMovement>();
            if (_movement)
                _movement.OnTurn -= OnTurn;
        }

        private void OnTurn(bool turn)
        {
            if (!spriteRenderer) return;

            if (delay != 0)
                spriteRenderer.flipX = _smoothTurn == 0 ? spriteRenderer.flipX : _smoothTurn < 0f;
            else
                spriteRenderer.flipX = !turn;
        }

        private float _smoothTurn;
        private bool _destroyed;

        private async UniTaskVoid FragmentedUpdate()
        {
            var prevTurn = 0f;
            while (true)
            {
                if (delay == 0) return;
                await Task.Delay(delay);
                if (!isActiveAndEnabled) continue;
                if (_destroyed) return;
                _smoothTurn = (prevTurn + _movement.TurnInFloat) / 2f;
                prevTurn = _smoothTurn;
            }
        }

        private void OnDestroy()
        {
            _destroyed = true;
        }
    }
}