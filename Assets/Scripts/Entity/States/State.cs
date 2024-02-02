using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States
{
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

        protected abstract IState Next { get; set; }

        IState IState.Next
        {
            get => Next;
            set => Next = value;
        }

        protected abstract Task Activate(Entity entity, IState previous); 
        Task IState.Activate(Entity entity, IState previous) =>  Activate(entity, previous);
    }
}