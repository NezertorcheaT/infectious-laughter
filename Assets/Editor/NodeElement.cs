using Entity.States;
using UnityEngine.UIElements;

namespace Editor
{
    public class NodeElement : Button
    {
        public State State;

        public override string text => State.Name;

        public void DrawCanvas(MeshGenerationContext ctx)
        {
            
        }
    }
}