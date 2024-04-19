namespace Entity.States
{
    /// <summary>
    /// эта кароч дерево, которое может быть обновлено<br />
    /// сейчас юзаю для сохранения скриптабл обж
    /// </summary>
    public interface IUpdatableAssetStateTree : IStateTree
    {
        /// <summary>
        /// обновить ассет
        /// </summary>
        void UpdateAsset();
        /// <summary>
        /// изменён ли ассет
        /// </summary>
        bool Unsaved { get; }
    }
}