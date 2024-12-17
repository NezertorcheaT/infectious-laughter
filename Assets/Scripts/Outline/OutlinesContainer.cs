using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Outline
{
    [CreateAssetMenu(fileName = "Outlines Container", menuName = "", order = 0)]
    public class OutlinesContainer : ScriptableObject
    {
        [SerializeField] private List<Sprite> spritesToGenerate = new();

#if UNITY_EDITOR
        [Button]
        private void Regenerate()
        {
            if (Directory.Exists(Outliner.NewMainPath))
                Directory.Delete(Outliner.NewMainPath, true);

            Cache = new List<OutlineType>();
            Cache.AddRange(spritesToGenerate
                .Select(sprite => new OutlineType
                {
                    Original = sprite,
                    New = Outliner.Regenerate(
                        sprite,
                        Path.Combine(Outliner.MainPath.Replace("Assets\\Drive", ""), AssetDatabase.GetAssetPath(sprite))
                            .Replace('/', '\\')
                    )
                })
                .Where(outlineType => outlineType.New is not null && outlineType.Original is not null)
            );
        }

        /// <summary>
        /// попытаться сгенерировать обводку, если отсутствует
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns>true - сгенерировано false - уже существует</returns>
        public bool TryGenerate(Sprite sprite)
        {
            if (Cache.Select(i => i.Original).Contains(sprite)) return false;
            if (!spritesToGenerate.Contains(sprite)) spritesToGenerate.Add(sprite);
            Cache.Add(new OutlineType
            {
                Original = sprite,
                New = Outliner.Regenerate(
                    sprite,
                    Path.Combine(Outliner.MainPath.Replace("Assets\\Drive", ""), AssetDatabase.GetAssetPath(sprite))
                        .Replace('/', '\\')
                )
            });
            return true;
        }
#endif

        [Serializable]
        public struct OutlineType
        {
            public Sprite Original;
            public Sprite New;
        }

        /// <summary>
        /// превращает спрайт в обводку
        /// </summary>
        /// <param name="original">оригинал спрайта</param>
        /// <returns>обводка без оригинала</returns>
        public Sprite this[Sprite original] => ToOutline(original);

        /// <summary>
        /// превращает спрайт в обводку
        /// </summary>
        /// <param name="original">оригинал спрайта</param>
        /// <returns>обводка без оригинала</returns>
        public static Sprite ToOutline(Sprite original)
        {
            var sprite = Instance.Cache.FirstOrDefault(t => t.Original == original).New;
            if (sprite is null)
                Debug.LogError(
                    $"Outliner не нашёл обводку для спрайта \"{original}\". Попробуйте обновить обводку с помощью \"File/Regenerate Outlines\"");
            return sprite;
        }

        /// <summary>
        /// НЕ ПЫТАЙТЕСЬ обращаться к этому свойству, оно защищено магической силой, и выдаст ошибку если запущено из билда
        /// </summary>
        [field: SerializeField]
#if UNITY_EDITOR
        public
#endif
            List<OutlineType> Cache
        {
            get;
#if UNITY_EDITOR
            private
#endif
            set;
        }

        public static OutlinesContainer Instance { get; set; }

        public void Initialize()
        {
            Instance ??= this;
        }

#if UNITY_EDITOR
        public void Reset()
        {
            Cache = new List<OutlineType>();
            Initialize();
        }
#endif
    }
}