using PropsImpact;
using UnityEngine;

namespace Entity.Abilities
{
    public class InteractivePropsUsing : Ability
    {
        [SerializeField] private float radius = 4.06f;

        public void UseInteractiveProps()
        {
            if (!Available()) return;
            var collidersInCircle = Physics2D.OverlapCircleAll(gameObject.transform.position, radius);

            for (var i = 0; i != collidersInCircle.Length; i++)
            {
                foreach (var prop in collidersInCircle[i].GetComponents<IUsableProp>())
                {
                    prop.Use();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}