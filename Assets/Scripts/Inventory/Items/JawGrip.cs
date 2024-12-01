using Cysharp.Threading.Tasks;
using Inventory.Input;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New JawGrip", menuName = "Inventory/Items/JawGrip", order = 0)]
    public class JawGrip : ScriptableObject, IUsableItem, IShopItem
    {
        public string Name => "JawGrip";
        public string Id => "il.jaw_grip";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;
        public Sprite SpriteForShop => spriteForShop;
        public int ItemCost => itemCost;

        [SerializeField, Min(1)] private int itemCost;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite spriteForShop;
        [SerializeField] private float timerUseMax = 15f;
        [SerializeField] private int radius = 15;

        [field: SerializeField] public int MaxStackSize { get; private set; }

        public async void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            slot.Count--;
            PlayerInventoryInput input = entity.FindAbilityByType<PlayerInventoryInput>();
            float timerUse = 0;
            float defaultInventoryRadius = input.MaxDistance;

            //Хватает предметы на расстоянии
            input.MaxDistance = radius;

            for (int i = 0; ; i++) {
                timerUse += Time.deltaTime;
                if (timerUse >= timerUseMax) { input.MaxDistance = defaultInventoryRadius; return; }
                await UniTask.Yield();
            }
            
        }
    }
}