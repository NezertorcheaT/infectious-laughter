using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Saving
{
    public static class GlobalFileSaver
    {
        public static string Path => Application.persistentDataPath;
    }
}