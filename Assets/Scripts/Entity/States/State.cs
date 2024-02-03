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

        protected abstract List<IState> Nexts { get; set; }

        List<IState> IState.Nexts
        {
            get
            {
                Nexts ??= new List<IState>(0);
                return Nexts;
            }
            set => Nexts = value;
        }

        protected abstract void Connect(IState state);
        void IState.Connect(IState state) => Connect(state);
        protected abstract void Disconnect(IState state);
        void IState.Disconnect(IState state) => Disconnect(state);

        protected abstract Task<IState> Activate(Entity entity, IState previous);
        Task<IState> IState.Activate(Entity entity, IState previous) => Activate(entity, previous);
    }
}