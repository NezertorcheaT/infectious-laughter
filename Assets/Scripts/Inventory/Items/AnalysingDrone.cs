using Cysharp.Threading.Tasks;
using Entity.Abilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New Analysing Drone", menuName = "Inventory/Items/Analysing Drone", order = 0)]
    public class AnalysingDrone : ScriptableObject, IUsableItem, IShopItem
    {
        public string Name => "Analysing Drone";
        public string Id => "il.analysing_drone";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;
        public int ItemCost => itemCost;

        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite spriteForShop;
        [SerializeField] private float timerUseMax = 10f;
        [SerializeField, Min(1)] private int itemCost;

        [FormerlySerializedAs("cameraDistanse")] [SerializeField, Min(1)]
        private float cameraDistance = 10;

        [field: SerializeField] public int MaxStackSize { get; private set; }

        public async void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            slot.Count--;

            var camera = entity.FindAbilityByType<CameraToItemsProvider>().Camera;
            var cameraDefaultVeiw = camera.m_Lens.OrthographicSize;
            float timerUse = 0;

            //Увеличить камеру на 100 
            camera.m_Lens.OrthographicSize = cameraDistance;

            for (var i = 0;; i++)
            {
                timerUse += Time.deltaTime;
                if (timerUse >= timerUseMax)
                {
                    camera.m_Lens.OrthographicSize = cameraDefaultVeiw;
                    return;
                }

                await UniTask.Yield();
            }
        }
    }
}