using UnityEngine;

namespace Entity.States
{
    public interface IPositionableStateTree : IStateTree
    {
        bool TrySetPosition(int id, Vector2 position);
        Vector2 GetPosition(int id);
        bool TryGetPosition(int id, ref Vector2 position);
    }
}