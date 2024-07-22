using CustomHelper;
using Entity.Abilities;
using Entity.States.StateObjects.Edits;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "Chase State", menuName = "AI Nodes/States/Chase State", order = 0)]
    public class ChaseState : State, IEditableState
    {
        [StateEdit] private ChaseStateEdit properties;

        public override string Name => "Chase";

        public override async Task<int> Activate(Entity entity, State previous)
        {
            var edit = properties;
            var nextId = edit.next;
            var collider2d = entity.GetComponent<Collider2D>();
            var moveAbility = entity.FindAbilityByType<EntityMovementHorizontalMove>();
            var direction = edit.initialDirection;
            Vector3 lastSeenPosition = Vector3.zero;
            bool playerInSight = false;

            if (!moveAbility) return nextId;

            for (; ; )
            {
                if (!entity) break;
                await Task.Yield();


                // ѕроверка игрока в поле зрени€
                RaycastHit2D hit = Physics2D.Raycast(entity.transform.position, direction ? Vector2.right : Vector2.left, edit.visionDistance, edit.playerLayer);


                var ray = new Ray(entity.transform.position + (Vector3)collider2d.offset + collider2d.bounds.size.Multiply(new Vector3(direction ? 1 : -1, -1f / 2f, 1)), Vector3.down);
                Debug.DrawRay(ray.origin, ray.direction * edit.rayDistance);


                // ќтрисовка луча дл€ отладки
                Vector2 rayDirection = direction ? Vector2.right : Vector2.left;
                Debug.DrawRay(entity.transform.position, rayDirection * edit.visionDistance, Color.red);

                if (hit.collider != null && Physics2D.Raycast(ray.origin, ray.direction, edit.rayDistance, edit.groundLayer))
                {
                    playerInSight = true;
                    lastSeenPosition = hit.point;
                }
                else
                {
                    playerInSight = false;
                }

                // ≈сли игрок в поле зрени€
                if (playerInSight)
                {
                    if (!Physics2D.Raycast(ray.origin, ray.direction, edit.rayDistance, edit.groundLayer)) { break; }
                    direction = (hit.point.x > entity.transform.position.x);
                    moveAbility.Move(direction ? 1 : -1);
                }
                else
                {
                    // ≈сли игрок не в поле зрени€, двигатьс€ к последней видимой точке
                    if (Vector3.Distance(entity.transform.position, lastSeenPosition) > 0.1f)
                    {
                        if(!Physics2D.Raycast(ray.origin, ray.direction, edit.rayDistance, edit.groundLayer)){ moveAbility.Move(0); break; }
                        direction = entity.transform.position.x < lastSeenPosition.x;
                        moveAbility.Move(direction ? 1 : -1);
                    }
                    else
                    {
                        // ¬ернутьс€ к патрулированию, если игрок не найден
                        break;
                    }
                }
            }

            return nextId;
        }

        Type IEditableState.GetTypeOfEdit() => typeof(ChaseStateEdit);
    }
}
