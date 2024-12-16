using UnityEngine;

namespace Inventory
{
    ///<summary>
    ///������� � ��������
    ///</summary>
    public interface IShopItem : INameableItem
    {
        /// <summary>
        /// ��� ������� �������� � ��������
        /// </summary>
        Sprite SpriteForShop { get; }

        /// <summary>
        /// ���� ��������
        /// </summary>
        int ItemCost { get; }
    }
}