using Cysharp.Threading.Tasks;
using Inventory.ImpactWorldItems;
using Inventory.Input;
using UnityEngine;
using UnityEngine.Windows;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New JawGrip Item", menuName = "Inventory/Items/JawGrip", order = 0)]
    public class JawGrip : ScriptableObject, IItem, IUsableItem
    {
        public string Name => "JawGrip";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private Sprite sprite;
        [SerializeField] private float timerUseMax = 15f;
        [SerializeField] private int radius = 15;

        [field: SerializeField] public int MaxStackSize { get; private set; }

        public async void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            PlayerInventoryInput input = entity.FindAbilityByType<PlayerInventoryInput>();
            float timerUse = 0;
            float defaultInventoryRadius = input.MaxDistance;

            //������� �������� �� ����������
            input.MaxDistance = radius;

            for (int i = 0; ; i++) {
                timerUse += Time.deltaTime;
                if (timerUse >= timerUseMax) { input.MaxDistance = defaultInventoryRadius; return; }
                await UniTask.Yield();
            }
        }
    }
}