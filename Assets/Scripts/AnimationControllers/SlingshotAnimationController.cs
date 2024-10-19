using PropsImpact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationControllers
{
    public class SlingshotAnimationController : MonoBehaviour
    {
        private PropsImpact.SlingshotImpact slingshotImpact;
        private Animator animator;
        private float chargeTime;

        public void Initialize(PropsImpact.SlingshotImpact _slingshotImpact, Animator _animator, float _chargeTime)
        {
            slingshotImpact = _slingshotImpact;
            animator = _animator;
            chargeTime = _chargeTime;

            slingshotImpact.StartCharge += StartChargeAnimation;
            slingshotImpact.Shot += SootAnimation;
        }

        private void StartChargeAnimation()
        {
            animator.SetBool("slingshot_is_shot", false);
            float animationSpeed = animator.GetCurrentAnimatorStateInfo(0).length / chargeTime;
            animator.SetFloat("SlingshotSpeed", animationSpeed);

            animator.SetBool("slingshot_charging", true);
            animator.Play("SlingshotCharge");
        }

        private void SootAnimation()
        {
            animator.SetBool("slingshot_charging", false);
            animator.Play("SlingshotAfterShot");

            animator.SetBool("slingshot_is_shot", true);

            slingshotImpact.StartCharge -= StartChargeAnimation;
            slingshotImpact.Shot -= SootAnimation;
        }
    }
}
