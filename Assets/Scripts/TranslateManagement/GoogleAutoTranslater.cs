using UnityEngine;
using Unity.Burst;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using UnityEngine.Networking;
using System.Reflection;
using System;


namespace TranslateManagement
{
    [BurstCompile]
    [CreateAssetMenu(fileName = "GoogleAutoTranslater", menuName = "TranslateManagement/Auto Translaters/Google AutoTranslater")]
    public class GoogleAutoTranslater : AutoTranslater
    {
        enum LoggingMode
        {
            None,
            ErrorsOnly,
            Full,
        }

        enum TranslateMode
        {
            Consistently,
            Bruteforce,
        }


        [Min(1)]
        [Tooltip("Number of translation attempts if the translation get error. If translation still ends in errors, increase this value")]
        [SerializeField] private int attemptsCount = 10;
        [Space]
        [Tooltip("Debug.Log mode")]
        [SerializeField] private LoggingMode logging = LoggingMode.Full;
        [Tooltip("Messages sending mode. Consistently - send all messages one by one. Bruteforce - sending all messages at once (may lead to errors due to Google DOS protection)")]
        [SerializeField] private TranslateMode sendMessagesMode;


        public bool BruteforceMode => sendMessagesMode == TranslateMode.Bruteforce;


        private readonly string successColor = "cyan"; // Color.cyan
        private readonly string retryColor = "yellow"; // Color.yellow
        private readonly string errorColor = "red"; // Color.red


        #region TranslateString

        public override async UniTask<string> TranslateAsync(string from, ApplicationLanguage to)
        {
            // Cancel if wrong language
            if (!ValidForTranslate(to))
            {
                throw new Exception($"{to} currently is not supported by Google Translate.");
            }



            // Creates url
            var url = string.Format("https://translate.google.com" + "/translate_a/single?client=gtx&dt=t&sl={0}&tl={1}&q={2}",
                "auto", to.ToHLCode(), WebUtility.UrlEncode(from));

            // Creates Web Request with url
            UnityWebRequest www = UnityWebRequest.Get(url);


            // Send request and wait for it
            await www.SendWebRequest();


            // Get text
            string response = www.downloadHandler.text;

            try
            {
                JArray jsonArray = JArray.Parse(response);
                response = jsonArray[0][0][0].ToString();
            }
            catch
            {
                throw new Exception("The process is not completed! Most likely, you made too many requests. In this case, the Google Translate API blocks access to the translation for a while.  Please try again later. Do not translate the text too often, so that Google does not consider your actions as spam");
            }

            return response;
        }

        #endregion

        #region TranslateAll

        public override async UniTask<Translation> TranslateAllAsync(Translation from, ApplicationLanguage to)
        {
            // Cancel if wrong language
            if (!ValidForTranslate(to))
            {
                throw new Exception($"{to} currently is not supported by Google Translate.");
            }



            // Gets all fields
            FieldInfo[] fields = typeof(Translation).GetFields();


            // Creates async handlers
            GoogleTranslateAsyncHandler[] handlers = new GoogleTranslateAsyncHandler[fields.Length];


            // Creates tasks collection for bruteforce
            UniTask[] handlersTasks = null;

            if (BruteforceMode)
            {
                handlersTasks = new UniTask[handlers.Length];
            }


            // Creates own translation
            Translation ownTranslation = new();

            for (int i = 0; i < fields.Length; i++)
            {
                // Current field
                FieldInfo field = fields[i];

                // Skip all non string fields
                if (field.FieldType != typeof(string))
                    continue;

                // Initialize handler
                handlers[i] = new GoogleTranslateAsyncHandler(language: to,
                                                              translationFrom: from,
                                                              translationTo: ownTranslation, 
                                                              field: field, 
                                                              googleAutoTranslater: this);

                // If bruteforce mode, fire translation and add to tasks collection
                if (BruteforceMode)
                    handlersTasks[i] = handlers[i].Start();

                // otherwise, wait for translate
                else
                    await handlers[i].Start();


                // We launch the next translation only after a delay so that Unity has time to
                // initialize this translation

                if (BruteforceMode)
                    await UniTask.Yield();
            }

            // Wait when all bruteforce tasks end
            if (BruteforceMode)
            {
                await UniTask.WhenAll(handlersTasks);
            }

            // Return own translation
            return ownTranslation;
        }

        #endregion


        #region AsyncHandler
        /// <summary>
        /// Class for deferred translation
        /// </summary>
        private class GoogleTranslateAsyncHandler
        {
            private readonly ApplicationLanguage language;
            private readonly FieldInfo field;
            private readonly object translationFrom;
            private readonly object translationTo;
            private readonly GoogleAutoTranslater googleAutoTranslater;

            public GoogleTranslateAsyncHandler(ApplicationLanguage language, FieldInfo field, object translationFrom, object translationTo, GoogleAutoTranslater googleAutoTranslater)
            {
                this.language = language;
                this.field = field;
                this.translationFrom = translationFrom;
                this.translationTo = translationTo;
                this.googleAutoTranslater = googleAutoTranslater;
            }

            public async UniTask Start()
            {
                // Cache current value
                string currentTranslationValue = (string)field.GetValue(translationFrom);

                string value;
                for (int a = 0; a < googleAutoTranslater.attemptsCount; a++)
                {
                    try
                    {
                        // Wait for translation
                        value = await googleAutoTranslater.TranslateAsync(currentTranslationValue, language);
                    }
                    catch (Exception exp)
                    {
                        // Retry if catched an error

                        // Log error if logging is not None
                        if (googleAutoTranslater.logging > 0)
                        {
                            // Checking if this is the last attempt
                            if (a + 1 < googleAutoTranslater.attemptsCount)
                            {
                                Debug.Log($"<color={googleAutoTranslater.retryColor}> " +
                                      $"The {field.Name} was not successfully translated. Attempt: {a} {Environment.NewLine} Error: </color> {exp}");
                            }
                            else
                            {
                                Debug.Log($"<color={googleAutoTranslater.errorColor}> " +
                                      $"The {field.Name} was skipped due to an error: </color> {exp}");
                            }
                        }


                        await UniTask.Yield();
                        continue;
                    }


                    // Set translated value if translate is success
                    field.SetValue(translationTo, value);

                    // Log warning if logging is not None
                    if (googleAutoTranslater.logging == LoggingMode.Full)
                        Debug.Log($"<color={googleAutoTranslater.successColor}> " + $"Successfully translated: {field.Name} </color>");

                    // Break the cycle if translate is success
                    break;
                }
            }
        }
#endregion

        public override bool ValidForTranslate(ApplicationLanguage language) 
            => !language.CheckForGoogleTranslateException();
    }
}
