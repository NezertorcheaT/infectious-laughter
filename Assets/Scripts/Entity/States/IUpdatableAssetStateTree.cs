namespace Entity.States
{
    public interface IUpdatableAssetStateTree : IStateTree
    {
        void UpdateAsset();
        bool Unsaved { get; }
    }
}