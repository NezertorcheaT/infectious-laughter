namespace Saving
{
    public interface IFileSaver<T>
    {
        public interface ISavable<TCovert>
        {
            TCovert Convert();
            ISavable<TCovert> Deconvert(TCovert converted);
        }

        void Save(ISavable<T> savable);
        T Read(string path);
    }
}