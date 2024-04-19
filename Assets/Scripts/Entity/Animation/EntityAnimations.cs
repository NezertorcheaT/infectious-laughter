using Entity.Controllers;
using Entity.States;
using UnityEngine;

namespace Entity.Animation
{
    /// <summary>
    /// без него не будут работать все состояния реализующие IAnimatableState
    /// вешать как способность
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [Tooltip("без него не будут работать все состояния реализующие IAnimatableState")]
    [AddComponentMenu("Entity/Abilities/States Animation Ability")]
    public class EntityAnimations : Ability
    {
        [Tooltip("обязателен для работы"), SerializeField]
        private Animator animator;


        public override void Initialize()
        {
            base.Initialize();
            if (!animator) animator = Entity.GetComponent<Animator>();
            if (!animator) return;

            if (Entity.Controller is ControllerAI controllerAi)
            {
                controllerAi.OnStateActivating += AnimateState;
            }
        }


        private void AnimateState(State state)
        {
            if (state is IAnimatableState animatableState)
            {
                animatableState.Animate(Entity, animator);
            }
        }
    }
}