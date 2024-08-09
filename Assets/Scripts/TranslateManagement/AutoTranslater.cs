using UnityEngine;
using Unity.Burst;
using Cysharp.Threading.Tasks;

namespace TranslateManagement
{
    [BurstCompile]
    public abstract class AutoTranslater : ScriptableObject
    {
        /// <summary>
        /// Performs translation of one string. If you don't override this, then it will call the asynchronous method
        /// </summary>
        public virtual string Translate(string from, ApplicationLanguage to)
        {
            // Get Task
            UniTask<string> task = TranslateAsync(from, to);

            while (task.Status == UniTaskStatus.Pending)
            {
                // Wait...
            }

            // Return task result
            return task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Performs translation. If you don't override this, then it will call the asynchronous method
        /// </summary>
        public virtual Translation TranslateAll(Translation from, ApplicationLanguage to)
        {
            // Get Task
            UniTask<Translation> task = TranslateAllAsync(from, to);

            while (task.Status == UniTaskStatus.Pending)
            {
                // Wait...
            }

            // Return task result
            return task.GetAwaiter().GetResult();
        }



        /// <summary>
        /// Performs translation of one string asynchronously. You must add the <seealso href="async"/> keyword in the implementation
        /// </summary>
        public abstract UniTask<string> TranslateAsync(string from, ApplicationLanguage to);

        /// <summary>
        /// Performs translation asynchronously. You must add the <seealso href="async"/> keyword in the implementation
        /// </summary>
        public abstract UniTask<Translation> TranslateAllAsync(Translation from, ApplicationLanguage to);



        /// <summary>
        /// Returns false if the autotranslator cannot translate this language
        /// </summary>
        public virtual bool ValidForTranslate(ApplicationLanguage language) => true;
    }
}
