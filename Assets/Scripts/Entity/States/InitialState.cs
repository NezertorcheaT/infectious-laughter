﻿using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "Initial State", menuName = "States/Initial State", order = 0)]
    public class InitialState : State,IOneExitState
    {
        public override string Name => "Root";

        public override int Id { get; set; }

        public override async Task<int> Activate(Entity entity, State previous, IEditableState.Properties properties) => 0;
    }
}