namespace Entity.States
{
    /// <summary>
    /// эта кароч дерево, которе имеет редактируемые состояния
    /// </summary>
    public interface IStateTreeWithEdits
    {
        /// <summary>
        /// попробовать получить параметры состояния по айдишнику
        /// </summary>
        /// <param name="id">айдишник состояния</param>
        /// <param name="edit">ссылка на параметры для записи</param>
        /// <returns>получилось получить или нет</returns>
        bool TryGetEdit(int id, ref IEditableState.Properties edit);

        /// <summary>
        /// гарантированно получить параметры состояния по айдишнику
        /// </summary>
        /// <param name="id">айдишник состояния</param>
        /// <returns>полученые параметры</returns>
        IEditableState.Properties GetEdit(int id);
    }
}