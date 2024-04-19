namespace Entity.States
{
    public interface IStateTreeWithEdits
    {
        bool TryGetEdit(int id, ref IEditableState.Properties edit);
        IEditableState.Properties GetEdit(int id);
    }
}