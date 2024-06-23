namespace Saving
{
    public class ConfigFileSaver : IFileSaver<string>
    {
        private static string Path => $"{GlobalFileSaver.Path}\\config.json";

        public void Save(IFileSaver<string>.ISavable savable) =>
            GlobalFileSaver.SaveToDrive(savable.Convert(), Path);

        public string Read(string path) => GlobalFileSaver.ReadFromDrive(Path);
    }
}