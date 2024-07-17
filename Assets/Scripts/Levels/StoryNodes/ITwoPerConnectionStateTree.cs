using Entity.States;
using JetBrains.Annotations;

namespace Levels.StoryNodes
{
    public interface ITwoPerConnectionStateTree<T> : IStateTree<T>
    {
        bool TryConnectToPort1(string idA, string idB);
        bool TryConnectToPort2(string idA, string idB);
        bool TryDisconnectAPort1(string idA, string idB);
        bool TryDisconnectAPort2(string idA, string idB);

        [CanBeNull]
        string GetPort1(string id);

        [CanBeNull]
        string GetPort2(string id);
    }
}