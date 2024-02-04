using System.Threading.Tasks;

namespace Entity.States
{
    public interface IState
    {
        /// <summary>
        /// бесполезно, пока я эдитор не сделаю
        /// </summary>
        string Name { get; }

        /// <summary>
        /// не менять пж
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// асинхронный метод собственно действия
        /// </summary>
        /// <param name="entity">над кем будет действие произведено</param>
        /// <param name="previous">предыдущее состояние, хз может нужно кому</param>
        /// <returns>должен вернуть номер следующего в массиве следующих состояний</returns>
        Task<int> Activate(Entity entity, IState previous);
    }
}