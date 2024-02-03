using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class NodeElement : VisualElement
    {
        float m_Radius = 100.0f;
        float m_Value = 40.0f;

        public float radius
        {
            get => m_Radius;
            set
            {
                m_Radius = value;
            }
        }

        public float Diameter => m_Radius * 2.0f;

        public float Value {
            get => m_Value;
            set { m_Value = value; MarkDirtyRepaint(); }
        }

        public NodeElement()
        {
            generateVisualContent += DrawCanvas;
        }

        private void DrawCanvas(MeshGenerationContext ctx)
        {
            
        }
    }
}