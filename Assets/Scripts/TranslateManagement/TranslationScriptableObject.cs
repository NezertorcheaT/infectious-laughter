using NaughtyAttributes;
using System;
using UnityEngine;
using Newtonsoft.Json;
using Unity.Burst;
using OnValueChangedAttribute = NaughtyAttributes.OnValueChangedAttribute;
using SFB;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TranslateManagement
{
    [BurstCompile]
    [CreateAssetMenu(fileName = "Translation_Language", menuName = "TranslateManagement/Translation", order = 1)]
    public class TranslationScriptableObject : ScriptableObject
    {
#if UNITY_EDITOR
        [field: OnValueChanged(nameof(Rename))]
#endif
        [field: JsonIgnore, SerializeField] public ApplicationLanguage Language { get; private set; } = ApplicationLanguage.English;

        public Translation Translation;

        #region Editor
#if UNITY_EDITOR

        private static readonly ExtensionFilter[] loadExplorerExtensions = new[] {
                    new ExtensionFilter("Text Files", "json", "txt" ),
                    new ExtensionFilter("Genious Files", "nahuy" ),
                    new ExtensionFilter("All Files", "*" ),
        };

        private static readonly ExtensionFilter[] saveExplorerExtensions = new[] {
                    new ExtensionFilter("Json", "json" ),
                    new ExtensionFilter("Text", "txt" ),
                    new ExtensionFilter("Genious", "nahuy" ),
        };

        private static readonly JsonSerializerSettings options = new()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
            Converters = new[]
            {
                new StringEnumConverter()
            },
        };





        public void Rename()
        {
            string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
            AssetDatabase.RenameAsset(assetPath, GetName());
            AssetDatabase.SaveAssets();
        }

        public string GetName()
        {
            return $"Translation_{Language}";
        }

        public void SetLanguage(ApplicationLanguage newLang)
        {
            Language = newLang;
            Rename();
        }

        /// <summary>
        /// Opens a file selection window and loads a translation from this file
        /// </summary>
        [Button]
        public void LoadTranslationFromFile()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", Application.dataPath,
                loadExplorerExtensions, false);

            if (paths == null || paths.Length == 0)
            {
                Debug.Log("Cancelled because you haven't selected any files!");
                return;
            }

            else if (paths.Length > 1)
            {
                Debug.Log("Cancelled because you have selected more than one file!");
                return;
            }



            try
            {
                // Deserialize json data from file
                Translation = JsonConvert.DeserializeObject<Translation>(File.ReadAllText(paths[0]), options);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Error while deserialization: {ex}");
                return;
            }


            Debug.Log("Success!");
        }

        /// <summary>
        /// Opens a file save window and saves the translation to this file
        /// </summary>
        [Button]
        public void SaveTranslationToFile()
        {
            string path = StandaloneFileBrowser.SaveFilePanel($"Save {Language}", Application.dataPath, name, saveExplorerExtensions);

            if (string.IsNullOrWhiteSpace(path))
            {
                Debug.Log("Canceled");
                return;
            }



            try
            {
                // Create file if it not exists
                if (!File.Exists(path))
                    File.Create(path);

                // Write json data into the file
                File.WriteAllText(path, JsonConvert.SerializeObject(Translation, options));
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Error while serialization: {ex}");
                return;
            }


            Debug.Log("Success!");
        }



        [Button("Auto translate (from English)")]
        public async UniTask AutoTranslate()
        {
            // Create translating task
            Translation.SetToEmptyFields(await AutoTranslateAsync(
                    TranslationConfig.Instance.GetTranslation(ApplicationLanguage.English).Translation, Language));

            // Report about successfully translate
            Debug.Log($"<color={nameof(Color.cyan)}> " +
                              $"The {Language} was successfully translated! </color>");
        }


        public static async UniTask<Translation> AutoTranslateAsync(Translation translation, ApplicationLanguage to)
        {
            if (translation == null)
                throw new ArgumentNullException(nameof(translation), 
                    "Translation is null!");

            if (TranslationConfig.Instance == null)
                throw new ArgumentNullException(nameof(TranslationConfig.Instance), 
                    "TranslationConfig not founded! Create it!");

            if (TranslationConfig.Instance.AutoTranslater == null)
                throw new ArgumentNullException(nameof(TranslationConfig.Instance.AutoTranslater), 
                    "AutoTranslater is null! Set AutoTranslater in TranslationConfig");



            return await TranslationConfig.Instance.AutoTranslater.TranslateAllAsync(translation, to);
        }



        [Button("Auto translate all languages (from English)")]
        public async UniTask TranslateAllLanguages()
        {
            string[] languages = Enum.GetNames(typeof(ApplicationLanguage));

            // Count of correctly translated languages
            uint correct = 0;

            for (int i = 0; i < languages.Length; i++)
            {
                // Display progress with current language
                DisplayProgressBar(i / (languages.Length + 1), languages[i]);
                try
                {
                    // Validate
                    if (!TranslationConfig.Instance.AutoTranslater.ValidForTranslate(languages[i].ToApplicationLanguage()))
                        throw new Exception("Language not supported by AutoTranslater");




                    // Get ScriptableObject if exists
                    if (TranslationConfig.Instance.TryGetTranslation(languages[i].ToApplicationLanguage(), out TranslationScriptableObject current))
                    {
                        // Wait for translate
                        await current.AutoTranslate();
                    }


                    else
                    {
                        // Create new language
                        current = CreateInstance<TranslationScriptableObject>();

                        // Set language to new ScriptableObject
                        current.SetLanguage(languages[i].ToApplicationLanguage());


                        // Wait for translate
                        await current.AutoTranslate();


                        // Get folder of this ScriptableObject and save new ScriptableObject in this folder
                        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
                        assetPath = Path.Combine(Path.GetDirectoryName(assetPath), current.GetName() + ".asset");

                        // Create asset
                        AssetDatabase.CreateAsset(current, assetPath);
                        AssetDatabase.SaveAssets();

                        // Add asset to config
                        TranslationConfig.Instance.AddTranslation(current);
                    }

                    // Increase the number of correctly translated languages ​​if the language was translated correctly
                    correct++;
                }
                catch (Exception exp)
                {
                    Debug.Log($"<color={nameof(Color.magenta)}> " +
                                      $"The {Language} was skipped due to an error: </color> {exp}");
                }
            }

            // Summarizing
            Debug.Log($"<color={nameof(Color.cyan)}> " +
                      $"Translate all ended! {correct} out of {languages.Length} languages ​​were translated correctly! </color>");

            // Stop displaying progress
            StopProgressBar();
        }

        private static void DisplayProgressBar(float progress, string translateTo)
        {
            EditorUtility.DisplayProgressBar($"Translating via {TranslationConfig.Instance.AutoTranslater.name}", $"Translating to {translateTo}", progress);
        }

        private static void StopProgressBar()
        {
            EditorUtility.ClearProgressBar();
        }
#endif
        #endregion
    }
}
