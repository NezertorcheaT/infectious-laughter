using System.Linq;
using Unity.Burst;
using UnityEngine;

namespace TranslateManagement
{
    [BurstCompile]
    public abstract class MultipleTranslater : Translater
    {
        [field: SerializeField] public string[] Names { get; private set; }


        protected override void Awake()
        {
            if (Names.Count(string.IsNullOrWhiteSpace) == Names.Length)
                return;

            base.Awake();
        }

        public virtual void SetName(string name, uint index, bool withInvoke = true)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            Names[index] = name;

            if (withInvoke)
                ChangeElement();
        }

        public string GetTranslationString(int index) => TranslateManager.GetTranslationString(Names[index]);
    }
}