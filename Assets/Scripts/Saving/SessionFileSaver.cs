using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            CheckDirectory();
            if (!(savable is Session session))
                throw new ArgumentException($"Provided savable '{savable}' is not a Session");
            GlobalFileSaver.SaveToDrive(savable.Convert(), CreatePath(session.ID));
        }

        public string Read(string path)
        {
            CheckDirectory();
            if (!path.Contains(Ending)) Debug.LogWarning($"File '{path}' probably not a Session, be careful");
            return GlobalFileSaver.ReadFromDrive(path);
        }

        public IEnumerable<string> GetSessionIDs() => Directory.EnumerateFiles(path).Select(Path.GetFileNameWithoutExtension);

        private void CheckDirectory()
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}