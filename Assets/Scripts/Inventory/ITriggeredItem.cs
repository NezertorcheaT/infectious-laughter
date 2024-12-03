namespace Inventory
{
    /// <summary>
    /// �������, ������� �����������(������������� ���������� �����)
    /// </summary>
    public interface ITriggeredItem : IItem
    {
        /// <summary>
        /// ������� ������������ ��������
        /// </summary>
        /// <param name="entity">�������� ����</param>
        /// <param name="inventory">������� ������</param>
        /// <param name="slot">������� ���</param>
        void Trigger(Entity.Entity entity, IInventory inventory, ISlot slot);
    }
}