using System.Collections.Generic;

namespace Entity.States
{
    public interface IStateTree
    {
        /// <summary>
        /// ну вы бля поняли, хэш
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        int Hash(State state);

        /// <summary>
        /// добавить состояние в общий пул
        /// </summary>
        /// <param name="state">состояние</param>
        void AddState(State state);

        /// <summary>
        /// попытаться соеденить два состояния
        /// </summary>
        /// <param name="idA">айди того куда подсоединять</param>
        /// <param name="idB">айди того что подсоединять</param>
        /// <returns>получилось или нет</returns>
        bool TryConnect(int idA, int idB);

        /// <summary>
        /// попытаться открепить два состояния
        /// </summary>
        /// <param name="idA">айди того откуда открепить</param>
        /// <param name="idB">айди того что открепить</param>
        /// <returns>получилось или нет</returns>
        bool TryDisconnect(int idA, int idB);

        /// <summary>
        /// попытаться открепить состояние от вообще всего
        /// </summary>
        /// <param name="id">айди того что открепить</param>
        /// <returns>получилось или нет</returns>
        bool TryDisconnect(int id);

        /// <summary>
        /// находит все айди состояний, в Next которых есть нужное
        /// </summary>
        /// <param name="id">айди состояния</param>
        /// <returns>массив айди</returns>
        int[] FindConnections(int id);

        /// <summary>
        /// попытаться безопасно удалить состояние из пула
        /// </summary>
        /// <param name="id">айди состояния</param>
        /// <returns>получилось или нет</returns>
        bool TryRemoveState(int id);

        /// <summary>
        /// проверить, является ли число корректны айди
        /// </summary>
        /// <param name="id">айди</param>
        /// <returns></returns>
        bool IsIdValid(int id);

        /// <summary>
        /// попытаться получить состояние по айди
        /// </summary>
        /// <param name="id">айди</param>
        /// <param name="state">ссылка на выходное значение состояния</param>
        /// <returns>получилось или нет</returns>
        bool TryGetState(int id, ref State state);

        /// <summary>
        /// получить состояние по айди на прямую
        /// </summary>
        /// <param name="id">айди</param>
        /// <returns>состояние</returns>
        State GetState(int id);

        /// <summary>
        /// получить состояния следующие после этого состояния
        /// </summary>
        /// <param name="id">айди</param>
        /// <returns>состояния</returns>
        State[] GetNextsTo(int id);

        /// <summary>
        /// получить начальное состояние
        /// не путать с GetState(0)
        /// это разные вещи
        /// </summary>
        /// <returns>состояние</returns>
        State First();

        /// <summary>
        /// все состояния в соответствии с их айди
        /// </summary>
        Dictionary<int, State> States { get; }
    }
}