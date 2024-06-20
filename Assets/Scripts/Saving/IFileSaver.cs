namespace Saving
{
    public interface IFileSaver<T>
    {
        public interface ISavable<TSavable>
        {
            TSavable Convert { get; }
        }

        void Save(ISavable<T> savable);
        T Read(string path);
    }
}