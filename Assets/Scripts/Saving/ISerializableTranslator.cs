namespace Saving
{
    public interface ISerializableTranslator<T>
    {
        T Serialize(object a);
        object Deserialize(T a);
    }
}