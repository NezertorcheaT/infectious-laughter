using Levels.StoryNodes;
using TMPro;
using UnityEngine;
using Zenject;

namespace Shop
{
    public class ShopNextLevel : MonoBehaviour
    {
        [Inject] private LevelManager levelManager;
        [SerializeField] private TextMeshProUGUI textGui;

        private void Start()
        {
            textGui.SetText($"Следующий уровень: {levelManager.CurrentLevel.LevelNumber}");
        }
    }
}