using Levels.StoryNodes;
using Outline;
using UnityEngine;
using Zenject;

namespace Shop
{
    public class ShopNextLevel : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private OutlineAutoGetter outline;
        [SerializeField] private Sprite[] numbers;
        [Inject] private LevelManager levelManager;

        private void Start()
        {
            renderer.sprite = numbers[Mathf.Clamp(levelManager.CurrentLevel.LevelNumber, 0, 4)];
            outline.UpdateOutline();
        }
    }
}