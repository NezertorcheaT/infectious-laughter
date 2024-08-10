using System;
using UnityEngine;

namespace TranslateManagement
{
    #region Exceptions
    public class NotSupportedSystemLanguageException : Exception
    {
        public readonly SystemLanguage Language;
        public NotSupportedSystemLanguageException(string message, SystemLanguage language) : base(message)
        {
            Language = language;
        }
    }
    public class NotSupportedApplicationLanguageException : Exception
    {
        public readonly ApplicationLanguage Language;
        public NotSupportedApplicationLanguageException(string message, ApplicationLanguage language) : base(message)
        {
            Language = language;
        }
    }
    #endregion
    public static class LanguagesConverter
    {
        #region IsGoogleTranslateException
        public static bool IsGoogleTranslateException(ApplicationLanguage applicationLanguage) // Google returns an error when sending these languages
        {
            return applicationLanguage switch
            {
                ApplicationLanguage.Unknown => true,
                ApplicationLanguage.Bemba => true,
                ApplicationLanguage.Bork => true,
                ApplicationLanguage.Breton => true,
                ApplicationLanguage.Cherokee => true,
                ApplicationLanguage.Faroese => true,
                ApplicationLanguage.Hacker => true,
                ApplicationLanguage.Interlingua => true,
                ApplicationLanguage.Kirundi => true,
                ApplicationLanguage.Klingon => true,
                ApplicationLanguage.Kongo => true,
                ApplicationLanguage.Lozi => true,
                ApplicationLanguage.Luo => true,
                ApplicationLanguage.Mauritian_Creole => true,
                ApplicationLanguage.Nigerian_Pidgin => true,
                ApplicationLanguage.Norwegian_Nynorsk => true,
                ApplicationLanguage.Occitan => true,
                ApplicationLanguage.Pirate => true,
                ApplicationLanguage.Romansh => true,
                ApplicationLanguage.Runyakitara => true,
                ApplicationLanguage.Scots => true,
                ApplicationLanguage.Setswana => true,
                ApplicationLanguage.Seychellois_Creole => true,
                ApplicationLanguage.Tonga => true,
                ApplicationLanguage.Tshiluba => true,
                ApplicationLanguage.Tumbuka => true,
                ApplicationLanguage.Wolof => true,
                _ => false
            };
        }
        #endregion

        #region SystemToHLCode
        public static string SystemToHLCode(SystemLanguage systemLanguage)
        {
            return systemLanguage switch
            {
                SystemLanguage.Afrikaans => "af",
                SystemLanguage.Arabic => "ar",
                SystemLanguage.Basque => "eu",
                SystemLanguage.Belarusian => "be",
                SystemLanguage.Bulgarian => "bg",
                SystemLanguage.Catalan => "ca",
                SystemLanguage.Chinese => "zh", // For Chinese, it's better to use the generic "zh" code, as there are variations.
                SystemLanguage.Czech => "cs",
                SystemLanguage.Danish => "da",
                SystemLanguage.Dutch => "nl",
                SystemLanguage.English => "en",
                SystemLanguage.Estonian => "et",
                SystemLanguage.Faroese => "fo",
                SystemLanguage.Finnish => "fi",
                SystemLanguage.French => "fr",
                SystemLanguage.German => "de",
                SystemLanguage.Greek => "el",
                SystemLanguage.Hebrew => "he",
                SystemLanguage.Hungarian => "hu",
                SystemLanguage.Icelandic => "is",
                SystemLanguage.Indonesian => "id",
                SystemLanguage.Italian => "it",
                SystemLanguage.Japanese => "ja",
                SystemLanguage.Korean => "ko",
                SystemLanguage.Latvian => "lv",
                SystemLanguage.Lithuanian => "lt",
                SystemLanguage.Norwegian => "no",
                SystemLanguage.Polish => "pl",
                SystemLanguage.Portuguese => "pt",
                SystemLanguage.Romanian => "ro",
                SystemLanguage.Russian => "ru",
                SystemLanguage.SerboCroatian => "sh",
                SystemLanguage.Slovak => "sk",
                SystemLanguage.Slovenian => "sl",
                SystemLanguage.Spanish => "es",
                SystemLanguage.Swedish => "sv",
                SystemLanguage.Thai => "th",
                SystemLanguage.Turkish => "tr",
                SystemLanguage.Ukrainian => "uk",
                SystemLanguage.Vietnamese => "vi",
                SystemLanguage.ChineseSimplified => "zh-CN",
                SystemLanguage.ChineseTraditional => "zh-TW",
                _ => throw new NotSupportedSystemLanguageException(
                    $"{systemLanguage} is not supported by Google Translate", systemLanguage)
            };
        }
        #endregion
        #region ApplicationToHLCode
        public static string ApplicationToHLCode(ApplicationLanguage language)
        {
            return language switch
            {
                ApplicationLanguage.Afrikaans => "af",
                ApplicationLanguage.Akan => "ak",
                ApplicationLanguage.Albanian => "sq",
                ApplicationLanguage.Amharic => "am",
                ApplicationLanguage.Arabic => "ar",
                ApplicationLanguage.Armenian => "hy",
                ApplicationLanguage.Azerbaijani => "az",
                ApplicationLanguage.Basque => "eu",
                ApplicationLanguage.Belarusian => "be",
                ApplicationLanguage.Bemba => "bem",
                ApplicationLanguage.Bengali => "bn",
                ApplicationLanguage.Bihari => "bh",
                ApplicationLanguage.Bork => "xx-bork",
                ApplicationLanguage.Bosnian => "bs",
                ApplicationLanguage.Breton => "br",
                ApplicationLanguage.Bulgarian => "bg",
                ApplicationLanguage.Cambodian => "km",
                ApplicationLanguage.Catalan => "ca",
                ApplicationLanguage.Cherokee => "chr",
                ApplicationLanguage.Chichewa => "ny",
                ApplicationLanguage.Chinese_Simplified => "zh-cn",
                ApplicationLanguage.Chinese_Traditional => "zh-tw",
                ApplicationLanguage.Corsican => "co",
                ApplicationLanguage.Croatian => "hr",
                ApplicationLanguage.Czech => "cs",
                ApplicationLanguage.Danish => "da",
                ApplicationLanguage.Dutch => "nl",
                ApplicationLanguage.Elmer => "elmer",
                ApplicationLanguage.English => "en",
                ApplicationLanguage.Esperanto => "eo",
                ApplicationLanguage.Estonian => "et",
                ApplicationLanguage.Ewe => "ee",
                ApplicationLanguage.Faroese => "fo",
                ApplicationLanguage.Filipino => "fil",
                ApplicationLanguage.Finnish => "fi",
                ApplicationLanguage.French => "fr",
                ApplicationLanguage.Frisian => "fy",
                ApplicationLanguage.Ga => "ga",
                ApplicationLanguage.Galician => "gl",
                ApplicationLanguage.Georgian => "ka",
                ApplicationLanguage.German => "de",
                ApplicationLanguage.Greek => "el",
                ApplicationLanguage.Guarani => "gn",
                ApplicationLanguage.Gujarati => "gu",
                ApplicationLanguage.Hacker => "xx-hacker",
                ApplicationLanguage.Haitian => "ht",
                ApplicationLanguage.Hausa => "ha",
                ApplicationLanguage.Hawaiian => "haw",
                ApplicationLanguage.Hebrew => "he",
                ApplicationLanguage.Hindi => "hi",
                ApplicationLanguage.Hungarian => "hu",
                ApplicationLanguage.Icelandic => "is",
                ApplicationLanguage.Igbo => "ig",
                ApplicationLanguage.Indonesian => "id",
                ApplicationLanguage.Interlingua => "ia",
                ApplicationLanguage.Irish => "ga",
                ApplicationLanguage.Italian => "it",
                ApplicationLanguage.Japanese => "ja",
                ApplicationLanguage.Javanese => "jv",
                ApplicationLanguage.Kannada => "kn",
                ApplicationLanguage.Kazakh => "kk",
                ApplicationLanguage.Kinyarwanda => "rw",
                ApplicationLanguage.Kirundi => "rn",
                ApplicationLanguage.Klingon => "xx-klingon",
                ApplicationLanguage.Kongo => "kg",
                ApplicationLanguage.Korean => "ko",
                ApplicationLanguage.Krio => "kri",
                ApplicationLanguage.Kurdish => "ku",
                ApplicationLanguage.Kurdish_Soranî => "ckb",
                ApplicationLanguage.Kyrgyz => "ky",
                ApplicationLanguage.Laothian => "lo",
                ApplicationLanguage.Latin => "la",
                ApplicationLanguage.Latvian => "lv",
                ApplicationLanguage.Lingala => "ln",
                ApplicationLanguage.Lithuanian => "lt",
                ApplicationLanguage.Lozi => "loz",
                ApplicationLanguage.Luganda => "lg",
                ApplicationLanguage.Luo => "ach",
                ApplicationLanguage.Macedonian => "mk",
                ApplicationLanguage.Malagasy => "mg",
                ApplicationLanguage.Malay => "ms",
                ApplicationLanguage.Malayalam => "ml",
                ApplicationLanguage.Maltese => "mt",
                ApplicationLanguage.Maori => "mi",
                ApplicationLanguage.Marathi => "mr",
                ApplicationLanguage.Mauritian_Creole => "mfe",
                ApplicationLanguage.Moldavian => "mo",
                ApplicationLanguage.Mongolian => "mn",
                ApplicationLanguage.Montenegrin => "sr-me",
                ApplicationLanguage.Nepali => "ne",
                ApplicationLanguage.Nigerian_Pidgin => "pcm",
                ApplicationLanguage.Northern_Sotho => "nso",
                ApplicationLanguage.Norwegian => "no",
                ApplicationLanguage.Norwegian_Nynorsk => "nn",
                ApplicationLanguage.Occitan => "oc",
                ApplicationLanguage.Oriya => "or",
                ApplicationLanguage.Oromo => "om",
                ApplicationLanguage.Pashto => "ps",
                ApplicationLanguage.Persian => "fa",
                ApplicationLanguage.Pirate => "xx-pirate",
                ApplicationLanguage.Polish => "pl",
                ApplicationLanguage.Portuguese_Brazil => "pt-br",
                ApplicationLanguage.Portuguese_Portugal => "pt-pt",
                ApplicationLanguage.Punjabi => "pa",
                ApplicationLanguage.Quechua => "qu",
                ApplicationLanguage.Romanian => "ro",
                ApplicationLanguage.Romansh => "rm",
                ApplicationLanguage.Runyakitara => "rn",
                ApplicationLanguage.Russian => "ru",
                ApplicationLanguage.Scots => "sco",
                ApplicationLanguage.Serbian => "sr",
                ApplicationLanguage.Serbo_Croatian => "sh",
                ApplicationLanguage.Sesotho => "st",
                ApplicationLanguage.Setswana => "tn",
                ApplicationLanguage.Seychellois_Creole => "crs",
                ApplicationLanguage.Shona => "sn",
                ApplicationLanguage.Sindhi => "sd",
                ApplicationLanguage.Sinhalese => "si",
                ApplicationLanguage.Slovak => "sk",
                ApplicationLanguage.Slovenian => "sl",
                ApplicationLanguage.Somali => "so",
                ApplicationLanguage.Spanish => "es",
                ApplicationLanguage.Spanish_LatinAmerican => "es-419",
                ApplicationLanguage.Sundanese => "su",
                ApplicationLanguage.Swahili => "sw",
                ApplicationLanguage.Swedish => "sv",
                ApplicationLanguage.Tajik => "tg",
                ApplicationLanguage.Tamil => "ta",
                ApplicationLanguage.Tatar => "tt",
                ApplicationLanguage.Telugu => "te",
                ApplicationLanguage.Thai => "th",
                ApplicationLanguage.Tigrinya => "ti",
                ApplicationLanguage.Tonga => "to",
                ApplicationLanguage.Tshiluba => "lua",
                ApplicationLanguage.Tumbuka => "tum",
                ApplicationLanguage.Turkish => "tr",
                ApplicationLanguage.Turkmen => "tk",
                ApplicationLanguage.Twi => "tw",
                ApplicationLanguage.Uighur => "ug",
                ApplicationLanguage.Ukrainian => "uk",
                ApplicationLanguage.Urdu => "ur",
                ApplicationLanguage.Uzbek => "uz",
                ApplicationLanguage.Vietnamese => "vi",
                ApplicationLanguage.Welsh => "cy",
                ApplicationLanguage.Wolof => "wo",
                ApplicationLanguage.Xhosa => "xh",
                ApplicationLanguage.Yiddish => "yi",
                ApplicationLanguage.Yoruba => "yo",
                ApplicationLanguage.Zulu => "zu",
                _ => throw new NotSupportedApplicationLanguageException($"{language} is not supported", language)
            };
        }
        #endregion

        #region SystemToApplicationLanguage
        public static ApplicationLanguage SystemToApplicationLanguage(SystemLanguage systemLanguage)
        {
            return systemLanguage switch
            {
                SystemLanguage.Unknown => ApplicationLanguage.Unknown,
                SystemLanguage.Afrikaans => ApplicationLanguage.Afrikaans,
                SystemLanguage.Arabic => ApplicationLanguage.Arabic,
                SystemLanguage.Basque => ApplicationLanguage.Basque,
                SystemLanguage.Belarusian => ApplicationLanguage.Belarusian,
                SystemLanguage.Bulgarian => ApplicationLanguage.Bulgarian,
                SystemLanguage.Catalan => ApplicationLanguage.Catalan,
                SystemLanguage.Chinese => ApplicationLanguage.Chinese_Traditional, // Could be either Simplified or Traditional. Defaulting to Traditional.
                SystemLanguage.Czech => ApplicationLanguage.Czech,
                SystemLanguage.Danish => ApplicationLanguage.Danish,
                SystemLanguage.Dutch => ApplicationLanguage.Dutch,
                SystemLanguage.English => ApplicationLanguage.English,
                SystemLanguage.Estonian => ApplicationLanguage.Estonian,
                SystemLanguage.Faroese => ApplicationLanguage.Faroese,
                SystemLanguage.Finnish => ApplicationLanguage.Finnish,
                SystemLanguage.French => ApplicationLanguage.French,
                SystemLanguage.German => ApplicationLanguage.German,
                SystemLanguage.Greek => ApplicationLanguage.Greek,
                SystemLanguage.Hebrew => ApplicationLanguage.Hebrew,
                SystemLanguage.Hungarian => ApplicationLanguage.Hungarian,
                SystemLanguage.Icelandic => ApplicationLanguage.Icelandic,
                SystemLanguage.Indonesian => ApplicationLanguage.Indonesian,
                SystemLanguage.Italian => ApplicationLanguage.Italian,
                SystemLanguage.Japanese => ApplicationLanguage.Japanese,
                SystemLanguage.Korean => ApplicationLanguage.Korean,
                SystemLanguage.Latvian => ApplicationLanguage.Latvian,
                SystemLanguage.Lithuanian => ApplicationLanguage.Lithuanian,
                SystemLanguage.Norwegian => ApplicationLanguage.Norwegian,
                SystemLanguage.Polish => ApplicationLanguage.Polish,
                SystemLanguage.Portuguese => ApplicationLanguage.Portuguese_Portugal, // Could be either Portugal or Brazil. Defaulting to Portugal.
                SystemLanguage.Romanian => ApplicationLanguage.Romanian,
                SystemLanguage.Russian => ApplicationLanguage.Russian,
                SystemLanguage.SerboCroatian => ApplicationLanguage.Serbo_Croatian,
                SystemLanguage.Slovak => ApplicationLanguage.Slovak,
                SystemLanguage.Slovenian => ApplicationLanguage.Slovenian,
                SystemLanguage.Spanish => ApplicationLanguage.Spanish,
                SystemLanguage.Swedish => ApplicationLanguage.Swedish,
                SystemLanguage.Thai => ApplicationLanguage.Thai,
                SystemLanguage.Turkish => ApplicationLanguage.Turkish,
                SystemLanguage.Ukrainian => ApplicationLanguage.Ukrainian,
                SystemLanguage.Vietnamese => ApplicationLanguage.Vietnamese,
                SystemLanguage.ChineseSimplified => ApplicationLanguage.Chinese_Simplified,
                SystemLanguage.ChineseTraditional => ApplicationLanguage.Chinese_Traditional,
                _ => throw new NotSupportedSystemLanguageException($"{systemLanguage} is not supported", systemLanguage)
            };
        }
        #endregion

        #region ParseToApplicationLanguage

        public static ApplicationLanguage ParseToApplicationLanguage(string language) =>
            Enum.Parse<ApplicationLanguage>(language);

        #endregion

        #region ConvertHLCodeToApplicationLanguage
        public static ApplicationLanguage HLCodeToApplication(string hlCode)
        {
            return hlCode switch
            {
                "af" => ApplicationLanguage.Afrikaans,
                "ak" => ApplicationLanguage.Akan,
                "sq" => ApplicationLanguage.Albanian,
                "am" => ApplicationLanguage.Amharic,
                "ar" => ApplicationLanguage.Arabic,
                "hy" => ApplicationLanguage.Armenian,
                "az" => ApplicationLanguage.Azerbaijani,
                "eu" => ApplicationLanguage.Basque,
                "be" => ApplicationLanguage.Belarusian,
                "bem" => ApplicationLanguage.Bemba,
                "bn" => ApplicationLanguage.Bengali,
                "bh" => ApplicationLanguage.Bihari,
                "xx-bork" => ApplicationLanguage.Bork,
                "bs" => ApplicationLanguage.Bosnian,
                "br" => ApplicationLanguage.Breton,
                "bg" => ApplicationLanguage.Bulgarian,
                "km" => ApplicationLanguage.Cambodian,
                "ca" => ApplicationLanguage.Catalan,
                "chr" => ApplicationLanguage.Cherokee,
                "ny" => ApplicationLanguage.Chichewa,
                "zh-cn" => ApplicationLanguage.Chinese_Simplified,
                "zh-tw" => ApplicationLanguage.Chinese_Traditional,
                "co" => ApplicationLanguage.Corsican,
                "hr" => ApplicationLanguage.Croatian,
                "cs" => ApplicationLanguage.Czech,
                "da" => ApplicationLanguage.Danish,
                "nl" => ApplicationLanguage.Dutch,
                "elmer" => ApplicationLanguage.Elmer,
                "en" => ApplicationLanguage.English,
                "eo" => ApplicationLanguage.Esperanto,
                "et" => ApplicationLanguage.Estonian,
                "ee" => ApplicationLanguage.Ewe,
                "fo" => ApplicationLanguage.Faroese,
                "fil" => ApplicationLanguage.Filipino,
                "fi" => ApplicationLanguage.Finnish,
                "fr" => ApplicationLanguage.French,
                "fy" => ApplicationLanguage.Frisian,
                "ga" => ApplicationLanguage.Irish, // ApplicationLanguage.Ga и ApplicationLanguage.Irish имеют одинаковый код "ga"
                "gl" => ApplicationLanguage.Galician,
                "ka" => ApplicationLanguage.Georgian,
                "de" => ApplicationLanguage.German,
                "el" => ApplicationLanguage.Greek,
                "gn" => ApplicationLanguage.Guarani,
                "gu" => ApplicationLanguage.Gujarati,
                "xx-hacker" => ApplicationLanguage.Hacker,
                "ht" => ApplicationLanguage.Haitian,
                "ha" => ApplicationLanguage.Hausa,
                "haw" => ApplicationLanguage.Hawaiian,
                "he" => ApplicationLanguage.Hebrew,
                "hi" => ApplicationLanguage.Hindi,
                "hu" => ApplicationLanguage.Hungarian,
                "is" => ApplicationLanguage.Icelandic,
                "ig" => ApplicationLanguage.Igbo,
                "id" => ApplicationLanguage.Indonesian,
                "ia" => ApplicationLanguage.Interlingua,
                "it" => ApplicationLanguage.Italian,
                "ja" => ApplicationLanguage.Japanese,
                "jv" => ApplicationLanguage.Javanese,
                "kn" => ApplicationLanguage.Kannada,
                "kk" => ApplicationLanguage.Kazakh,
                "rw" => ApplicationLanguage.Kinyarwanda,
                "rn" => ApplicationLanguage.Kirundi,
                "xx-klingon" => ApplicationLanguage.Klingon,
                "kg" => ApplicationLanguage.Kongo,
                "ko" => ApplicationLanguage.Korean,
                "kri" => ApplicationLanguage.Krio,
                "ku" => ApplicationLanguage.Kurdish,
                "ckb" => ApplicationLanguage.Kurdish_Soranî,
                "ky" => ApplicationLanguage.Kyrgyz,
                "lo" => ApplicationLanguage.Laothian,
                "la" => ApplicationLanguage.Latin,
                "lv" => ApplicationLanguage.Latvian,
                "ln" => ApplicationLanguage.Lingala,
                "lt" => ApplicationLanguage.Lithuanian,
                "loz" => ApplicationLanguage.Lozi,
                "lg" => ApplicationLanguage.Luganda,
                "ach" => ApplicationLanguage.Luo,
                "mk" => ApplicationLanguage.Macedonian,
                "mg" => ApplicationLanguage.Malagasy,
                "ms" => ApplicationLanguage.Malay,
                "ml" => ApplicationLanguage.Malayalam,
                "mt" => ApplicationLanguage.Maltese,
                "mi" => ApplicationLanguage.Maori,
                "mr" => ApplicationLanguage.Marathi,
                "mfe" => ApplicationLanguage.Mauritian_Creole,
                "mo" => ApplicationLanguage.Moldavian,
                "mn" => ApplicationLanguage.Mongolian,
                "sr-me" => ApplicationLanguage.Montenegrin,
                "ne" => ApplicationLanguage.Nepali,
                "pcm" => ApplicationLanguage.Nigerian_Pidgin,
                "nso" => ApplicationLanguage.Northern_Sotho,
                "no" => ApplicationLanguage.Norwegian,
                "nn" => ApplicationLanguage.Norwegian_Nynorsk,
                "oc" => ApplicationLanguage.Occitan,
                "or" => ApplicationLanguage.Oriya,
                "om" => ApplicationLanguage.Oromo,
                "ps" => ApplicationLanguage.Pashto,
                "fa" => ApplicationLanguage.Persian,
                "xx-pirate" => ApplicationLanguage.Pirate,
                "pl" => ApplicationLanguage.Polish,
                "pt-br" => ApplicationLanguage.Portuguese_Brazil,
                "pt-pt" => ApplicationLanguage.Portuguese_Portugal,
                "pa" => ApplicationLanguage.Punjabi,
                "qu" => ApplicationLanguage.Quechua,
                "ro" => ApplicationLanguage.Romanian,
                "rm" => ApplicationLanguage.Romansh,
                "ru" => ApplicationLanguage.Russian,
                "sco" => ApplicationLanguage.Scots,
                "sr" => ApplicationLanguage.Serbian,
                "sh" => ApplicationLanguage.Serbo_Croatian,
                "st" => ApplicationLanguage.Sesotho,
                "tn" => ApplicationLanguage.Setswana,
                "crs" => ApplicationLanguage.Seychellois_Creole,
                "sn" => ApplicationLanguage.Shona,
                "sd" => ApplicationLanguage.Sindhi,
                "si" => ApplicationLanguage.Sinhalese,
                "sk" => ApplicationLanguage.Slovak,
                "sl" => ApplicationLanguage.Slovenian,
                "so" => ApplicationLanguage.Somali,
                "es" => ApplicationLanguage.Spanish,
                "es-419" => ApplicationLanguage.Spanish_LatinAmerican,
                "su" => ApplicationLanguage.Sundanese,
                "sw" => ApplicationLanguage.Swahili,
                "sv" => ApplicationLanguage.Swedish,
                "tg" => ApplicationLanguage.Tajik,
                "ta" => ApplicationLanguage.Tamil,
                "tt" => ApplicationLanguage.Tatar,
                "te" => ApplicationLanguage.Telugu,
                "th" => ApplicationLanguage.Thai,
                "ti" => ApplicationLanguage.Tigrinya,
                "to" => ApplicationLanguage.Tonga,
                "lua" => ApplicationLanguage.Tshiluba,
                "tum" => ApplicationLanguage.Tumbuka,
                "tr" => ApplicationLanguage.Turkish,
                "tk" => ApplicationLanguage.Turkmen,
                "tw" => ApplicationLanguage.Twi,
                "ug" => ApplicationLanguage.Uighur,
                "uk" => ApplicationLanguage.Ukrainian,
                "ur" => ApplicationLanguage.Urdu,
                "uz" => ApplicationLanguage.Uzbek,
                "vi" => ApplicationLanguage.Vietnamese,
                "cy" => ApplicationLanguage.Welsh,
                "wo" => ApplicationLanguage.Wolof,
                "xh" => ApplicationLanguage.Xhosa,
                "yi" => ApplicationLanguage.Yiddish,
                "yo" => ApplicationLanguage.Yoruba,
                "zu" => ApplicationLanguage.Zulu,
                _ => throw new NotImplementedException($"HL code {hlCode} is not supported")
            };
        }
        #endregion

        #region Extensions

        public static bool CheckForGoogleTranslateException(this ApplicationLanguage systemLanguage)
            => IsGoogleTranslateException(systemLanguage);

        public static string ToHLCode(this SystemLanguage systemLanguage)
            => SystemToHLCode(systemLanguage);

        public static string ToHLCode(this ApplicationLanguage applicationLanguage)
            => ApplicationToHLCode(applicationLanguage);

        public static ApplicationLanguage ToApplicationLanguage(this SystemLanguage systemLanguage)
            => SystemToApplicationLanguage(systemLanguage);

        public static ApplicationLanguage ToApplicationLanguage(this string parse)
            => ParseToApplicationLanguage(parse);

        public static ApplicationLanguage HLToApplicationLanguage(this string hlCode)
            => HLCodeToApplication(hlCode);
        #endregion
    }
}

