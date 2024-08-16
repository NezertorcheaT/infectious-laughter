namespace Entity.Abilities
{
    public interface IJumpableAbility : IInitializeByEntity
    {
        float JumpTime { get; }
        void Perform();
    }
}