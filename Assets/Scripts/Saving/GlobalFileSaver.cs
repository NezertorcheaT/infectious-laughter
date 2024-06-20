using System.IO;
using UnityEngine;

namespace Saving
{
    public static class GlobalFileSaver
    {
        public static string Path => Application.persistentDataPath;

        public static string ReadFromDrive(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException($"Path: '{path}'");

            var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            var stream = new StreamReader(fs);
            var str = stream.ReadToEnd();
            stream.Close();
            fs.Close();
            return str;
        }

        public static void SaveToDrive(string content, string path)
        {
            if (File.Exists(path)) File.Delete(path);
            var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            var stream = new StreamWriter(fs);
            stream.WriteLine(content);
            stream.Flush();
            stream.Close();
            fs.Close();
        }
    }
}