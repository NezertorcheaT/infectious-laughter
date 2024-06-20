using System;
using System.Collections.Generic;
using System.Linq;

namespace Saving
{
    public class SessionCreator
    {
        private SessionFileSaver _saver;
        public Session Current { get; private set; }

        public SessionCreator(SessionFileSaver saver)
        {
            _saver = saver;
        }

        public void SaveCurrentSession()
        {
            _saver.Save(Current);
        }

        public Session NewSession()
        {
            var session = new Session();
            Current = session;
            SaveCurrentSession();
            return session;
        }

        public Session LoadSession(string ID)
        {
            var path = SessionFileSaver.CreatePath(ID);
            if (!GetAvailableSessionIDs().ToArray().Contains(path))
                throw new ArgumentException($"Session '{ID}' does not exist");
            var session = new Session().Deconvert(_saver.Read(path), _saver) as Session;
            Current = session;
            return session;
        }

        public IEnumerable<string> GetAvailableSessionIDs() => _saver.GetSessionIDs();
    }
}