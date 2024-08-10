using TMPro;
using Unity.Burst;
using UnityEngine;

namespace TranslateManagement
{
    [BurstCompile]
    [AddComponentMenu("Translaters/Dropdown Translater")]
    public class DropdownTranslater : MultipleTranslater
    {
        private TMP_Dropdown dropdown;

        protected override void Awake()
        {
            dropdown = GetComponent<TMP_Dropdown>();

            base.Awake();
        }

        public override void ChangeElement()
        {
            if (Names == null || Names.Length == 0)
                return;

            for (var i = 0; i < Names.Length && i < dropdown.options.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(Names[i]))
                    continue;

                dropdown.options[i].text = GetTranslationString(i);
            }

            dropdown.captionText.SetText(dropdown.options[dropdown.value].text);
        }
    }
}