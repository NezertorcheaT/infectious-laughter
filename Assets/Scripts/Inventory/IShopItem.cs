using UnityEngine;

namespace Inventory
{
    ///<summary>
    ///предмет в магазине
    ///</summary>
    public interface IShopItem : IItem
    {
        /// <summary>
        /// как предмет выглядит в магазине
        /// </summary>
        Sprite SpriteForShop { get; }
        /// <summary>
        /// цена предмета
        /// </summary>
        int ItemCost { get; }
    }
}
