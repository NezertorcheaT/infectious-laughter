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
            foreach (var type in TypeCache.GetTypesDerivedFrom<IItem>())
            {
                if (type.IsInterface) continue;
                _items.Add(AssetDatabase.LoadAssetAtPath(
                        AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"t:{type.Name}").First()), type) as
                    ScriptableObject);
            }
#endif
        }

        public IItem IdToItem(string id)
        {
            if (id == string.Empty || id == " " || id is null) return null;
            var a = _items.Find(i => (i as IItem)?.Id == id);
            if (a is null)
                Debug.LogError($"Item Provider не смог найти предмет по айди {id}, попробуйте ресетнуть провайдер из редактора, перед тем как тестировать новые предметы");
            return a as IItem;
        }

        public void Initialize()
        {
            Instance = this;
        }
    }
}