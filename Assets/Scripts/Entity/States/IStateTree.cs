using System.Collections.Generic;

namespace Entity.States
{
    public interface IStateTree
    {
        int Hash(IState state);
        void AddState(IState state);
        bool TryConnect(int idA, int idB);
        bool TryDisconnect(int idA, int idB);
        bool TryDisconnect(int id);
        int[] FindConnections(int id);
        bool TryRemoveState(int id);
        bool IsIdValid(int id);
        bool TryGetState(int id, ref IState state);
        IState GetState(int id);
        IState First();
        Dictionary<int, IState> States { get; }
    }
}