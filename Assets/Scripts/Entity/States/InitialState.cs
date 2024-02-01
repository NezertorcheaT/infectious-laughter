using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "Initial State", menuName = "States/Initial State", order = 0)]
    public class InitialState : ScriptableObject, IState
    {
        string IState.Name => throw new System.NotImplementedException();

        int IState.Id { get; set; }

        IState IState.Next { get; set; }

        void IState.Activate(Entity entity, IState previous)
        {
        }
    }
}