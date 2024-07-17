using Cinemachine;
using Cysharp.Threading.Tasks;
using Entity.Abilities;
using Inventory.Input;
using NaughtyAttributes;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "New AnalysingDrone Item", menuName = "Inventory/Items/AnalysingDrone", order = 0)]
    public class AnalysingDrone : ScriptableObject, IItem, IUsableItem
    {
        public string Name => "AnalysingDrone";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private Sprite sprite;
        [SerializeField] private float timerUseMax = 10f;
        [SerializeField, Min(1)] private float cameraDistanse = 10;
        [field: SerializeField] public int MaxStackSize { get; private set; }

        public async void Use(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            CinemachineVirtualCamera camera = entity.FindAbilityByType<PlayerGetCamera>().Camera;

            float cameraDefaultVeiw = camera.m_Lens.OrthographicSize;
            float timerUse = 0;

            //Увеличить камеру на 100 
            camera.m_Lens.OrthographicSize = cameraDistanse;

            for (int i = 0; ; i++)
            {
                timerUse += Time.deltaTime;
                if (timerUse >= timerUseMax) { camera.m_Lens.OrthographicSize = cameraDefaultVeiw; return; }
                await UniTask.Yield();
            }
        }
    }
}