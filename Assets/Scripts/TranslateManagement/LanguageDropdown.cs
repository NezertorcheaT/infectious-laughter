using TMPro;
using Unity.Burst;
using System.Linq;
using UnityEngine;

namespace TranslateManagement.UI
{
    [BurstCompile]
    public class LanguageDropdown : MonoBehaviour
    {
        protected TMP_Dropdown dropdown;

        protected void Awake()
        {
            dropdown = GetComponent<TMP_Dropdown>();

            // Select all languages as list of strings
            var languagesNames = TranslationConfig.Instance.GetTranslations().Select(i => i.Language.ToString())
                .ToList();

            dropdown.ClearOptions();
            dropdown.AddOptions(languagesNames);
        }

        protected void Start()
        {
            dropdown.value = dropdown.options // Select indexes
                .Select(o => dropdown.options.IndexOf(o)) // Find option with game language
                .FirstOrDefault(s => dropdown.options[s].text.Equals(TranslateManager.GameLanguage.ToString())); // Return index
        }
    }
}