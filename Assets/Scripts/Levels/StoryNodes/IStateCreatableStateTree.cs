using Entity.States;

namespace Levels.StoryNodes
{
    public interface ITwoPerConnectionStateTree<T> : IStateTree<T>
    {
        bool TryConnectToPort1(string idA, string idB);
        bool TryConnectToPort2(string idA, string idB);
    }
}