namespace Inventory
{
    /// <summary>
    /// �������, ������� �����������(������������� ���������� �����)
    /// </summary>
    public interface ITriggeredItem : IItem
    {
        void Trigger();
    }
}