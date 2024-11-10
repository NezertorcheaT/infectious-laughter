using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outline
{
    [CreateAssetMenu(fileName = "Outlines Container", menuName = "", order = 0)]
    public class OutlinesContainer : ScriptableObject
    {
        [Serializable]
        public struct OutlineType
        {
            public Sprite Original;
            public string Path;
        }

        public Sprite this[Sprite original]
        {
            get
            {
                var sprite = Outliner
                    .GetOutlined(Cache.FirstOrDefault(t => t.Original.texture == original.texture).Path)
                    .FirstOrDefault(t => t.name == original.name);
                if (sprite is null)
                    Debug.LogError($"Outliner не нашёл обводку для спрайта \"{original}\". Попробуйте обновить обводку с помощью \"File/Regenerate Outlines\"");
                return sprite;
            }
        }

        /// <summary>
        /// превращает спрайт в обводку
        /// </summary>
        /// <param name="original">оригинал спрайта</param>
        /// <returns>обводка без оригинала</returns>
        public static Sprite ToOutline(Sprite original)
        {
            var sprite = Outliner
                .GetOutlined(Instance.Cache.FirstOrDefault(t => t.Original.texture == original.texture).Path)
                .FirstOrDefault(t => t.name == original.name);
            if (sprite is null)
                Debug.LogError($"Outliner не нашёл обводку для спрайта \"{original}\". Попробуйте обновить обводку с помощью \"File/Regenerate Outlines\"");
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