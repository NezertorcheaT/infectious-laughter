using UnityEngine;
using Unity.Burst;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using NaughtyAttributes;

namespace TranslateManagement
{
    [BurstCompile]
    [CreateAssetMenu(fileName = "TranslationConfig", menuName = "Scripts/Translation Config", order = 2)]
    public class TranslationConfig : ScriptableObject
    {
        [SerializeField] private List<TranslationScriptableObject> translations;

        [field: Expandable]
        [field: SerializeField]
        public AutoTranslater AutoTranslater { get; private set; }


        public static TranslationConfig Instance
        {
            get
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                    return Resources.LoadAll<TranslationConfig>("")[0];

                var GUID = AssetDatabase.FindAssets($"t:{nameof(TranslationConfig)}");
                var assetPath = GUID.Length > 0 ? AssetDatabase.GUIDToAssetPath(GUID[0]) : null;

                return assetPath != null ? AssetDatabase.LoadAssetAtPath<TranslationConfig>(assetPath) : null;
#endif
                return Resources.LoadAll<TranslationConfig>("")[0];
            }
        }


        // Sets only one Instance (Singletone)
        private void Awake()
        {
#if UNITY_EDITOR
            if (AssetDatabase.FindAssets($"t:{nameof(TranslationConfig)}").Length <= 1)
                return;

            var assetPath = AssetDatabase.GetAssetPath(GetInstanceID());

            AssetDatabase.MoveAssetToTrash(assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Can be maximum 1 {nameof(TranslationConfig)}");
#endif
        }

        /// <summary>
        /// Looks for a translation and returns null if not found
        /// </summary>
        public TranslationScriptableObject GetTranslation(ApplicationLanguage language) =>
            translations.FirstOrDefault(t => t.Language == language);

        /// <summary>
        /// Adds translation if it not already exists
        /// </summary>
        public void AddTranslation(TranslationScriptableObject translation)
        {
            if (translations.Contains(translation))
                return;

            translations.Add(translation);
        }

        /// <summary>
        /// Return false if translation is null
        /// </summary>
        public bool TryGetTranslation(ApplicationLanguage language, out TranslationScriptableObject translation)
        {
            translation = GetTranslation(language);
            return translation != null;
        }

        public IReadOnlyCollection<TranslationScriptableObject> GetTranslations() => translations;
    }
}