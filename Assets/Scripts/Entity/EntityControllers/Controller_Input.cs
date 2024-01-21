namespace Scripts.Entity
{
    public class Controller_Input : Controller
    {
        private Controls actions;
        
        // Cache
        private EntityMovement_1DMove moveAbility;
        private EntityMovement_Jump jumpAbility;

        public override void Initialize()
        {
            base.Initialize();
            actions = new Controls();

            moveAbility = Entity.FindAbilityByType<EntityMovement_1DMove>();
            jumpAbility = Entity.FindAbilityByType<EntityMovement_Jump>();

            OnEnable();
        }

        private void OnEnable()
        {
            if (actions == null) return;
            actions.Enable();

            Entity.OnFixedUpdate += Move;
            actions.Gameplay.Jump.performed += ctx => jumpAbility.Jump();
        }
        private void OnDisable()
        {
            if (actions == null) return;
            actions.Disable();

            Entity.OnFixedUpdate -= Move;
            actions.Gameplay.Jump.performed -= ctx => jumpAbility.Jump();
        }

        private void Move() => moveAbility.Move(actions.Gameplay.Move.ReadValue<float>());
    }
}