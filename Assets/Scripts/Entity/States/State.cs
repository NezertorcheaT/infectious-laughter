using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States
{
    [Serializable]
    public abstract class State : ScriptableObject, IState
    {
        string IState.Name => Name;
        protected abstract string Name { get; }

        int IState.Id
        {
            get => Id;
            set => Id = value;
        }

        protected abstract int Id { get; set; }
        protected abstract Task<int> Activate(Entity entity, IState previous);
        Task<int> IState.Activate(Entity entity, IState previous) => Activate(entity, previous);
    }
}