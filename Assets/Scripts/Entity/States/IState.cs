using System.Collections.Generic;
using System.Threading.Tasks;

namespace Entity.States
{
    public interface IState
    {
        string Name { get; }
        int Id { get; set; }
        Task<int> Activate(Entity entity, IState previous);
    }
}