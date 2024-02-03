using System.Collections.Generic;
using System.Threading.Tasks;

namespace Entity.States
{
    public interface IState
    {
        string Name { get; }
        int Id { get; set; }
        List<IState> Nexts { get; set; }
        void Connect(IState state);
        void Disconnect(IState state);
        Task<IState> Activate(Entity entity, IState previous);
    }
}