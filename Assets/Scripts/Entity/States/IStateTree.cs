using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.States
{
    /// <summary>
    /// ДЕРЕВОООООО<br />
    /// АХУЕЕЕЕЕТЬ<br /> <br />
    /// </summary>
    /// <typeparam name="T">Тип ноды</typeparam>
    public interface IStateTree<T>
    {
        /// <summary>
        /// ну вы бля поняли, хэш
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        string Hash(T state);

        /// <summary>
        /// добавить состояние в общий пул
        /// </summary>
        /// <param name="state">состояние</param>
        /// <returns>айдиншик нового состояния</returns>
        string AddState(T state);

        /// <summary>
        /// попытаться соеденить два состояния
        /// </summary>
        /// <param name="idA">айди того куда подсоединять</param>
        /// <param name="idB">айди того что подсоединять</param>
        /// <returns>получилось или нет</returns>
        bool TryConnect(string idA, string idB);

        /// <summary>
        /// попытаться открепить два состояния
        /// </summary>
        /// <param name="idA">айди того откуда открепить</param>
        /// <param name="idB">айди того что открепить</param>
        /// <returns>получилось или нет</returns>
        bool TryDisconnect(string idA, string idB);

        /// <summary>
        /// попытаться открепить состояние от вообще всего
        /// </summary>
        /// <param name="id">айди того что открепить</param>
        /// <returns>получилось или нет</returns>
        bool TryDisconnect(string id);

        /// <summary>
        /// находит все айди состояний, в Next которых есть нужное
        /// </summary>
        /// <param name="id">айди состояния</param>
        /// <returns>массив айди</returns>
        string[] FindConnections(string id);

        /// <summary>
        /// попытаться безопасно удалить состояние из пула
        /// </summary>
        /// <param name="id">айди состояния</param>
        /// <returns>получилось или нет</returns>
        bool TryRemoveState(string id);

        /// <summary>
        /// проверить, является ли число корректны айди
        /// </summary>
        /// <param name="id">айди</param>
        /// <returns></returns>
        bool IsIdValid(string id);

        /// <summary>
        /// попытаться получить состояние по айди
        /// </summary>
        /// <param name="id">айди</param>
        /// <param name="state">ссылка на выходное значение состояния</param>
        /// <returns>получилось или нет</returns>
        bool TryGetState(string id, ref T state);

        /// <summary>
        /// получить состояние по айди на прямую
        /// </summary>
        /// <param name="id">айди</param>
        /// <returns>состояние</returns>
        T GetState(string id);

        /// <summary>
        /// получить состояния следующие после этого состояния
        /// </summary>
        /// <param name="id">айди</param>
        /// <returns>состояния</returns>
        string[] GetNextsTo(string id);

        /// <summary>
        /// а имеются ли следующие состояния
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsNextsTo(string id);

        /// <summary>
        /// получить начальное состояние
        /// не путать с <code>GetState(0)</code>
        /// это разные вещи
        /// </summary>
        /// <returns>состояние</returns>
        T First();

        /// <summary>
        /// эээ кароч если вы стейт ещё не создали, ооооон создаст его за вас
        /// </summary>
        /// <param name="type">тип стейта</param>
        /// <returns>объект стейта</returns>
        ScriptableObject CreateMissingStateObject(Type type);

        /// <summary>
        /// все состояния в соответствии с их айди
        /// </summary>
        Dictionary<string, T> Nodes { get; }
    }
}