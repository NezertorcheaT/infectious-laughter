using Unity.Burst;
using UnityEngine;

namespace TranslateManagement
{
    [BurstCompile]
    public abstract class Translater : MonoBehaviour
    {
        protected bool Valid => isActiveAndEnabled && TranslateManager.Initialized;

        protected virtual void Awake()
        {
            if (Valid)
                ChangeElement();
        }

        protected virtual void OnEnable()
        {
            TranslateManager.GameLanguageChanged += ChangeElement;

            if (Valid)
                ChangeElement();
        }

        protected virtual void OnDisable()
        {
            TranslateManager.GameLanguageChanged -= ChangeElement;
        }

        public abstract void ChangeElement();
    }
}