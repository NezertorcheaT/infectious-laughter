using UnityEngine;

namespace Entity.States
{
    public interface IZoomableStateTree<T> : IStateTree<T>
    {
        Vector3 Position { get; set; }
        Vector3 Scale { get; set; }
        Quaternion Rotation { get; set; }
    }
}