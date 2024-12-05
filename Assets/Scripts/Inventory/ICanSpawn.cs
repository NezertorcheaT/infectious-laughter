namespace Inventory
{
    /// <summary>
    /// этот предмет имеет функционал спавна объектов, требующих зависимости
    /// </summary>
    public interface ICanSpawn : IItem
    {
        /// <summary>
        /// именно эта штука позволит внедрять зависимости в объекты
        /// </summary>
        ItemAdderVerifier Verifier { get; set; }
    }
}