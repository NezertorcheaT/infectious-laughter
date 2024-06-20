namespace Saving
{
    public interface ISerializableTranslator<T>
    {
        IFileSaver<T>.ISavable<T> Serialize(object a);
        object Deserialize(T a);
    }
}