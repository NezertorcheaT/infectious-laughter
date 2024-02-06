using System.Threading.Tasks;
using UnityEngine;

namespace Entity.EntityAnimation
{
    public interface IAnimatableState
    {
        /// <summary>
        /// асинхронный метод, который будет выполняться паралельно активации состояния
        /// изза этого пожалуйста контролируйте время его протикания.
        /// использовать для анимации.
        /// чтобы такие методы выполнялись нужно чтобы на сущности висела специальная способность
        /// </summary>
        /// <param name="entity">на ком весит дерево</param>
        /// <param name="animator">аниматор сущности</param>
        /// <returns></returns>
        public Task Animate(Entity entity, Animator animator);
    }
}