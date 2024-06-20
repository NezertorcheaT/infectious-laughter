namespace Saving
{
    public class SessionFileSaver:IFileSaver<string>
    {
        public void Save(IFileSaver<string>.ISavable<string> savable)
        {
            throw new System.NotImplementedException();
        }

        public string Read(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}