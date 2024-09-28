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

        public Sprite this[Sprite original] =>
            Outliner.GetOutlined(Cache.FirstOrDefault(t => t.Original.texture == original.texture).Path)
                .FirstOrDefault(t => t.name == original.name);

        /// <summary>
        /// превращает спрайт в обводку
        /// </summary>
        /// <param name="original">оригинал спрайта</param>
        /// <returns>обводка без оригинала</returns>
        public static Sprite ToOutline(Sprite original) =>
            Outliner.GetOutlined(Instance.Cache.FirstOrDefault(t => t.Original.texture == original.texture).Path)
                .FirstOrDefault(t => t.name == original.name);

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

        public void Reset()
        {
            Cache = new List<OutlineType>();
            Initialize();
        }
    }
}