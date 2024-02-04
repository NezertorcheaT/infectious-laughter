using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.States
{
    [Serializable]
    public abstract class State : ScriptableObject, IState
    {
        /// <summary>
        /// бесполезно, пока я эдитор не сделаю
        /// </summary>
        string IState.Name => Name;
        /// <summary>
        /// бесполезно, пока я эдитор не сделаю
        /// </summary>
        protected abstract string Name { get; }

        /// <summary>
        /// не менять пж
        /// </summary>
        int IState.Id
        {
            get => Id;
            set => Id = value;
        }

        /// <summary>
        /// не менять пж
        /// </summary>
        protected abstract int Id { get; set; }
        /// <summary>
        /// асинхронный метод собственно действия
        /// </summary>
        /// <param name="entity">над кем будет действие произведено</param>
        /// <param name="previous">предыдущее состояние, хз может нужно кому</param>
        /// <returns>должен вернуть номер следующего в массиве следующих состояний</returns>
        protected abstract Task<int> Activate(Entity entity, IState previous);
        /// <summary>
        /// асинхронный метод собственно действия
        /// </summary>
        /// <param name="entity">над кем будет действие произведено</param>
        /// <param name="previous">предыдущее состояние, хз может нужно кому</param>
        /// <returns>должен вернуть номер следующего в массиве следующих состояний</returns>
        Task<int> IState.Activate(Entity entity, IState previous) => Activate(entity, previous);
    }
}