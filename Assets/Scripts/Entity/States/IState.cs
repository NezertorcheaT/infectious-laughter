using System.Threading.Tasks;

namespace Entity.States
{
    public interface IState
    {
        string Name { get; }
        int Id { get; set; }
        IState Next { get; set; }
        void Activate(Entity entity, IState previous);
    }
}