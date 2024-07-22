using CustomHelper;
using Entity.Abilities;
using Entity.States.StateObjects.Edits;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "Patrolling State", menuName = "AI Nodes/States/Patrolling State", order = 0)]
    public class PatrollingState : State, IEditableState
    {
        [StateEdit] private PatrollingStateEdit properties;
        public override string Name => "Patrolling";

        public override async Task<int> Activate(Entity entity, State previous)
        {
            var edit = properties;
            var nextId = edit.next;
            var collider2d = entity.GetComponent<Collider2D>();
            var moveAbility = entity.FindAbilityByType<EntityMovementHorizontalMove>();
            var direction = edit.initialDirection;

            if (!moveAbility) return nextId;

            for (; ; )
            {
                if (!entity) break;
                await Task.Yield();

                //Важно сделать проверку, на стелс туту. От стелся будет зависить дальность луча. 


                // Проверка на наличие игрока
                RaycastHit2D hit = Physics2D.Raycast(entity.transform.position, direction ? Vector2.right : Vector2.left, edit.visionDistance, edit.playerLayer);

                // Отрисовка луча для отладки
                Vector2 rayDirection = direction ? Vector2.right : Vector2.left;
                Debug.DrawRay(entity.transform.position, rayDirection * edit.visionDistance, Color.red);

                if (hit.collider != null)
                {
                    // Переключиться в состояние преследования
                    return nextId; 
                }

                // Патрулирование
                var ray = new Ray(
                    entity.transform.position + (Vector3)collider2d.offset +
                    collider2d.bounds.size.Multiply(new Vector3(direction ? 1 : -1, -1f / 2f, 1)),
                    Vector3.down);
                if (!Physics2D.Raycast(ray.origin, ray.direction, edit.rayDistance, edit.groundLayer))
                    direction = !direction;
                Debug.DrawRay(ray.origin, ray.direction * edit.rayDistance);

                moveAbility.Move(direction ? 1 : -1);
            }

            return nextId;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(PatrollingStateEdit);
    }
}
