using TMPro;
using Unity.Burst;
using UnityEngine;

namespace TranslateManagement
{
    [BurstCompile]
    [AddComponentMenu("Translaters/TextMeshPro Translater")]
    public class TextTranslater : SingleTranslater
    {
        private TMP_Text text;

        protected override void Awake()
        {
            text = GetComponent<TMP_Text>();

            base.Awake();
        }

        public override void ChangeElement()
        {
            text.SetText(TranslationString);
            print($"Text translated: {TranslationString}");
        }
    }
}