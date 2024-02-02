using System.Threading.Tasks;

namespace Entity.States
{
    public interface IState
    {
        string Name { get; }
        int Id { get; set; }
        IState Next { get; set; }
        Task Activate(Entity entity, IState previous);
    }
}