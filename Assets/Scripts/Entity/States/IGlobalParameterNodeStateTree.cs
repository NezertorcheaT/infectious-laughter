using System.Runtime.CompilerServices;

namespace Entity.States
{
    /// <summary>
    /// это дерево, стейты которого имеют глобальный для дерева параметр<br />
    /// желательно использовать только для редактора
    /// </summary>
    public interface IGlobalParameterNodeStateTree<T, T2> : IStateTree<T> where T2 : ITuple
    {
        /// <summary>
        /// попытаться задать параметры стейта по айдишнику
        /// </summary>
        /// <param name="id">айдишник</param>
        /// <param name="parameters">параметры</param>
        /// <returns></returns>
        bool TrySetParameters(string id, T2 parameters);

        /// <summary>
        /// гарантировано получить параметры стейта по айдишнику
        /// </summary>
        /// <param name="id">айдишник</param>
        /// <returns>параметры</returns>
        T2 GetParameters(string id);

        /// <summary>
        /// попытаться получить параметры стейта по айдишнику
        /// </summary>
        /// <param name="id">айдишник</param>
        /// <param name="parameters">параметры</param>
        /// <returns>получилось или нет</returns>
        bool TryGetParameters(string id, ref T2 parameters);
    }
}