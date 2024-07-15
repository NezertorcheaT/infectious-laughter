namespace Entity.States
{
    /// <summary>
    /// эта кароч дерево, которе имеет редактируемые состояния
    /// </summary>
    public interface IStateTreeWithEdits : IStateTree<State>
    {
        /// <summary>
        /// попробовать получить параметры состояния по айдишнику
        /// </summary>
        /// <param name="id">айдишник состояния</param>
        /// <param name="edit">ссылка на параметры для записи</param>
        /// <returns>получилось получить или нет</returns>
        bool TryGetEdit(string id, ref EditableStateProperties edit);

        /// <summary>
        /// гарантированно получить параметры состояния по айдишнику
        /// </summary>
        /// <param name="id">айдишник состояния</param>
        /// <returns>полученые параметры</returns>
        EditableStateProperties GetEdit(string id);
    }
}