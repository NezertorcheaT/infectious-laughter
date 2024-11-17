using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item Provider", menuName = "Inventory/Item Provider", order = 0)]
    public class ItemsProvider : ScriptableObject, IInitializable
    {
        [SerializeField] private List<ScriptableObject> _items;

        //эхх, обидно, в скриптабл обжекты инжекты прокидывать нельзя
        public static ItemsProvider Instance;

        private void Reset()
        {
            Instance = this;
#if UNITY_EDITOR
            foreach (var type in
                     TypeCache.GetTypesDerivedFrom<IItem>()
                         .Concat(TypeCache.GetTypesDerivedFrom<IUsableItem>())
                         .Concat(TypeCache.GetTypesDerivedFrom<ICanSpawn>())
                    )
            {
                if (type.IsInterface) continue;
                var asset = AssetDatabase.FindAssets($"t:{type.Name}").FirstOrDefault();
                if (asset is null) continue;
                var item = AssetDatabase.LoadAssetAtPath(
                        AssetDatabase.GUIDToAssetPath(asset), type) as
                    ScriptableObject;
                if (!_items.Contains(item))
                    _items.Add(item);
            }
#endif
        }

        public IItem IdToItem(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            var a = _items.Find(i => (i as IItem)?.Id == id);
            if (a is null)
                Debug.LogError(
                    $"Item Provider не смог найти предмет по айди {id}, попробуйте ресетнуть провайдер из редактора, перед тем как тестировать новые предметы");
            return a as IItem;
        }

        public void Initialize()
        {
            Instance = this;
        }
    }
}