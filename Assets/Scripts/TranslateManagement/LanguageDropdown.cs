using System.Collections.Generic;
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
            List<string> languagesNames = new(from t in TranslationConfig.Instance.GetTranslations()
                                              select t.Language.ToString());

            dropdown.ClearOptions();
            dropdown.AddOptions(languagesNames);
        }

        protected void Start()
        {
            dropdown.value = dropdown.options
                                        .Select(o => dropdown.options.IndexOf(o)) // Select indexes
                                        .Where(s => dropdown.options[s].text.Equals(TranslateManager.GameLanguage.ToString())) // Find option with game language
                                        .FirstOrDefault(); // Return index
        }
    }
}

