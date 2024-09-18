using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace UI
{
    public class MouseImage : MonoBehaviour
    {
        [SerializeField] private Sprite mouseDowned;
        [SerializeField] private Sprite mouseUpped;
        private Image _selfImage;
        private bool _visible;

        public bool Visible
        {
            get => _visible;
            set
            {
                _selfImage.enabled = value;
                _visible = value;
            }
        }

        private void Start()
        {
            _selfImage = gameObject.GetComponent<Image>();
            Cursor.visible = false;
            Visible = true;
        }

        private void Update()
        {
            _selfImage.sprite = Mouse.current.leftButton.isPressed ? mouseDowned : mouseUpped;
            gameObject.transform.position = Mouse.current.position.ReadValue();
        }
    }
}