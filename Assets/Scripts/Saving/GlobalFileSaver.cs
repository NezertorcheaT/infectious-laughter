using System.IO;
using UnityEngine;

namespace Saving
{
    public static class GlobalFileSaver
    {
        public static string Path => Application.persistentDataPath;
        /*public static Configs ReadFromDrive()
        {
            if (!File.Exists(Path))
                new Configs().SaveToDrive();
        
            var fs = new FileStream(Path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            var stream = new StreamReader(fs);
            var str = JsonNode
                .Parse(stream.ReadToEnd())
                .Deserialize<Configs>()!;
            stream.Close();
            fs.Close();
            return str;
        }

        public void SaveToDrive()
        {
            File.Delete(Path);
            var fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            var file = JsonSerializer.Serialize(this);
            var stream = new StreamWriter(fs);
            stream.Flush();
            stream.WriteLine(file);
            stream.Close();
            fs.Close();
        }*/
    }
}