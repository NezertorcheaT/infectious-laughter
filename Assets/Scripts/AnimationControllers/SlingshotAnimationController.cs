using UnityEngine;

namespace AnimationControllers
{
    public class SlingshotAnimationController : MonoBehaviour
    {
        private PropsImpact.SlingshotImpact _slingshotImpact;
        private Animator _animator;
        private float _chargeTime;

        private static readonly int SlingshotIsShot = Animator.StringToHash("slingshot_is_shot");
        private static readonly int SlingshotSpeed = Animator.StringToHash("SlingshotSpeed");
        private static readonly int SlingshotCharging = Animator.StringToHash("slingshot_charging");
        private bool _initialized;

        public void Initialize(PropsImpact.SlingshotImpact slingshotImpact, Animator animator, float chargeTime)
        {
            if (_initialized) return;
            _initialized = true;
            _slingshotImpact = slingshotImpact;
            _animator = animator;
            _chargeTime = chargeTime;

            _slingshotImpact.StartCharge += StartChargeAnimation;
            _slingshotImpact.Shot += SootAnimation;
        }

        private void StartChargeAnimation()
        {
            if (!_animator) return;
            _animator.SetBool(SlingshotIsShot, false);
            _animator.SetFloat(SlingshotSpeed, _animator.GetCurrentAnimatorStateInfo(0).length / _chargeTime);
            _animator.SetBool(SlingshotCharging, true);
            _animator.Play("SlingshotCharge");
        }

        private void SootAnimation()
        {
            if (_animator)
            {
                _animator.SetBool(SlingshotCharging, false);
                _animator.Play("SlingshotAfterShot");
                _animator.SetBool(SlingshotIsShot, true);
            }

            _slingshotImpact.StartCharge -= StartChargeAnimation;
            _slingshotImpact.Shot -= SootAnimation;
        }
    }
}