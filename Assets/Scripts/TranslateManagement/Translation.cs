using NaughtyAttributes;
using System;
using System.Linq;
using System.Reflection;
using Unity.Burst;
using UnityEngine;

namespace TranslateManagement
{
    [BurstCompile, Serializable]
    public sealed class Translation
    {
        [Header("Global")]
        public string example_string;

        [ResizableTextArea]
        public string example_long_string;

        public string example_dropdown_string_1;
        public string example_dropdown_string_2;
        public string example_dropdown_string_3;
        public string example_dropdown_string_4;



        #region Methods
        /// <summary>
        /// Applies the new translation only to empty fields
        /// </summary>
        public void SetToEmptyFields(Translation newTranslation)
        {
            FieldInfo[] fields = typeof(Translation).GetFields();
            FieldInfo[] emptyFields = fields
                                      .Where(f => string.IsNullOrWhiteSpace(f.GetValue(this) as string))
                                      .ToArray();

            // Set values only to empty fields
            foreach (FieldInfo field in emptyFields)
            {
                field.SetValue(this, field.GetValue(newTranslation));
            }
        }
        #endregion
    }
}
