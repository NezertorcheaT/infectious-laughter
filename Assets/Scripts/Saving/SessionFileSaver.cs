using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Saving
{
    public class SessionFileSaver : IFileSaver<string>
    {
        private static string path => Path.Combine(GlobalFileSaver.Path, "saves");
        private static string Ending => ".session";

        public static string CreatePath(string ID) => Path.Combine(path, ID + Ending);

        public void Save(IFileSaver<string>.ISavable savable)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!(savable is Session session))
                throw new ArgumentException($"Provided savable '{savable}' is not a Session");
            GlobalFileSaver.SaveToDrive(savable.Convert(), CreatePath(session.ID));
        }

        public string Read(string path)
        {
            if (!Directory.Exists(SessionFileSaver.path))
                Directory.CreateDirectory(SessionFileSaver.path);

            if (!path.Contains(Ending)) Debug.LogWarning($"File '{path}' probably not a Session, be careful");
            return GlobalFileSaver.ReadFromDrive(path);
        }

        public IEnumerable<string> GetSessionIDs() => Directory.EnumerateFiles(path);
    }
}