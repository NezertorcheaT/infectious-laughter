using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Saving
{
    /// <summary>
    /// штука, контролирующая сессии
    /// </summary>
    public class SessionFactory
    {
        private SessionFileSaver _saver;

        /// <summary>
        /// загруженная сейчас сессия
        /// </summary>
        public Session Current { get; private set; }

        public SessionFactory(SessionFileSaver saver)
        {
            _saver = saver;
        }

        /// <summary>
        /// типа для сохранения текущей сессии
        /// </summary>
        public void SaveCurrentSession()
        {
            _saver.Save(Current);
        }

        /// <summary>
        /// для правильного создания новой сессии
        /// </summary>
        /// <returns>текущая сессия</returns>
        public Session NewSession()
        {
            var session = new Session();
            Current = session;
            SaveCurrentSession();
            return session;
        }

        /// <summary>
        /// для загрузки сессии по id
        /// </summary>
        /// <param name="ID">id</param>
        /// <returns>текущая сессия</returns>
        /// <exception cref="ArgumentException">сессия по id не существует</exception>
        public Session LoadSession(string ID)
        {
#if UNITY_EDITOR
            Debug.Log(SessionFileSaver.CreatePath(ID));
#endif
            if (!GetAvailableSessionIDs().ToArray().Contains(ID))
                throw new ArgumentException($"Session '{ID}' does not exist");
            var session = new Session().Deconvert(_saver.Read(SessionFileSaver.CreatePath(ID)), _saver) as Session;
            Current = session;
            return session;
        }

        /// <summary>
        /// для получения доступных для загрузки сессий
        /// </summary>
        /// <returns>кучка доступных id</returns>
        public IEnumerable<string> GetAvailableSessionIDs() => _saver.GetSessionIDs();
    }
}