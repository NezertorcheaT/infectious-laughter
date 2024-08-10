using System;
using Unity.Burst;
using UnityEngine;

namespace TranslateManagement
{
    [BurstCompile]
    public static partial class TranslateManager
    {
        public static ApplicationLanguage GameLanguage { get; private set; } = ApplicationLanguage.Unknown;
        public static event Action GameLanguageChanged;

        public static Translation Translation { get; private set; }

        public static bool Initialized { get; private set; }

        #region Load & Save

        public static Translation LoadTranslation(ApplicationLanguage language)
        {
            if (TranslationConfig.Instance.TryGetTranslation(language, out var result))
                return result.Translation;

            throw new Exception($"{language} not founded");
        }

        #endregion

        /// <summary>
        /// Returns the system language
        /// </summary>
        public static ApplicationLanguage GetSystemLanguage()
        {
#if YG_PLUGIN_YANDEX_GAME
            return YG.YandexGame.lang.ToApplicationLanguage();
#else
            return Application.systemLanguage.ToApplicationLanguage();
#endif
        }

        /// <summary>
        /// Sets new language
        /// </summary>
        public static bool ChangeLanguage(ApplicationLanguage newLanguage, bool withInvoke = true)
        {
            if (GameLanguage == newLanguage)
                return false;

            Translation = LoadTranslation(newLanguage);

            // If Translation not founded, stop process
            if (Translation == null)
                return false;

            Initialized = true;

            GameLanguage = newLanguage;

            TranslateCacher.RebuildData();

            if (withInvoke) GameLanguageChanged?.Invoke();

            return true;
        }


        public static string GetTranslationString(string name) => TranslateCacher.Get(name);
    }
}