namespace Saving
{
    public interface IFileSaver<T>
    {
        public interface ISavable<TSavable>
        {
            TSavable Convert { get; set; }
        }

        void Save(ISavable<T> savable);
        T Read(string path);
    }
}