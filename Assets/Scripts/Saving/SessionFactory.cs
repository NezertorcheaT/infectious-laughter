using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <returns>текущаяя сессия</returns>
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
        /// <returns>текущаяя сессия</returns>
        /// <exception cref="ArgumentException">сессия по id не существует</exception>
        public Session LoadSession(string ID)
        {
            var path = SessionFileSaver.CreatePath(ID);
            if (!GetAvailableSessionIDs().ToArray().Contains(path))
                throw new ArgumentException($"Session '{ID}' does not exist");
            var session = new Session().Deconvert(_saver.Read(path), _saver) as Session;
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