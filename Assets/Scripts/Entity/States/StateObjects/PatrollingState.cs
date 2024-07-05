﻿using System;
using System.Threading.Tasks;
using Entity.Abilities;
using Entity.States.StateObjects.Edits;
using UnityEngine;

namespace Entity.States.StateObjects
{
    [CreateAssetMenu(fileName = "Patrolling State", menuName = "AI Nodes/States/Patrolling State", order = 0)]
    public class PatrollingState : State, IEditableState
    {
        public override string Name => "Patrolling";

        public override async Task<int> Activate(Entity entity, State previous, EditableStateProperties properties)
        {
            var edit = properties as PatrollingStateEdit;
            var nextId = edit.next;
            var coll = entity.GetComponent<Collider2D>();
            var moveAbility = entity.FindAbilityByType<EntityMovementHorizontalMove>();
            var direction = edit.initialDirection;

            if (!moveAbility) return nextId;

            for (;;)
            {
                if (!entity) break;
                await Task.Yield();

                var ray = new Ray(
                    entity.transform.position + (Vector3) coll.offset +
                    coll.bounds.size.Multiply(new Vector3(direction ? 1 : -1, -1f / 2f, 1)),
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