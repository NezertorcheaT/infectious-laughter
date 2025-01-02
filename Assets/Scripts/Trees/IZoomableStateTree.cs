using UnityEngine;

namespace Trees
{
    public interface IZoomableStateTree<T> : IStateTree<T>
    {
        Vector3 Position { get; set; }
        Vector3 Scale { get; set; }
        Quaternion Rotation { get; set; }
    }
}