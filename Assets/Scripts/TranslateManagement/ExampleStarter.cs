using NaughtyAttributes;
using System.Linq;
using TMPro;
using TranslateManagement;
using UnityEngine;

public class ExampleStarter : MonoBehaviour
{
    private enum LanguageSetMode
    {
        System,
        Manual
    }

    [SerializeField] private LanguageSetMode setMode;

    [ShowIf(nameof(setMode), LanguageSetMode.Manual)] [Dropdown(nameof(AvailableLanguages))] [SerializeField]
    private string languageManual;

    [Space] [SerializeField] private TMP_Dropdown languagesDropdown;


    private string[] AvailableLanguages => TranslationConfig.Instance.GetTranslations()
        .Select(t => t.Language.ToString())
        .ToArray();

    private int lastLanguagesDropdownValue;


    [Button("Change Language")]
    public void Awake()
    {
        if (!Application.isPlaying)
        {
            Debug.Log("Вы можете изменить язык только когда игра запущена!");
            return;
        }

        lastLanguagesDropdownValue = languagesDropdown.value;

        ApplicationLanguage language;

        language = setMode == LanguageSetMode.System
            ? Application.systemLanguage.ToApplicationLanguage()
            : languageManual.ToApplicationLanguage();


        // Для запуска системы перевода нам нужна только одна строчка
        TranslateManager.ChangeLanguage(language);
    }

    private bool first = true;

    public void ChangeLanguage(int languageIndex)
    {
        if (first)
        {
            first = false;
            return;
        }

        // Parse option to ApplicationLanguage
        var language = languagesDropdown.options[languageIndex].text.ToApplicationLanguage();

        var success = TranslateManager.ChangeLanguage(language);

        if (success)
            lastLanguagesDropdownValue = languagesDropdown.value;
        else
            languagesDropdown.value = lastLanguagesDropdownValue;
    }
}