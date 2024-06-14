namespace Inventory
{
    
    public interface ICanSpawn : IItem
    {
        ItemAdderVerifier Verifier { get; set; }
    }
}