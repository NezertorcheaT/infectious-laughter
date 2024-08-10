using Unity.Burst;
using UnityEngine;

namespace TranslateManagement
{
    [BurstCompile]
    public abstract class SingleTranslater : Translater
    {
        [field: SerializeField] public string Name { get; private set; }
        public string TranslationString => TranslateManager.GetTranslationString(Name);


        protected override void Awake()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return;

            base.Awake();
        }

        public virtual void SetName(string name, bool withInvoke = true)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            Name = name;

            if (withInvoke)
                ChangeElement();
        }
    }
}