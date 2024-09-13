using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Entity.Abilities
{
    public class InteractivePropsUsing : Ability
    {
        [SerializeField] private float _radius;

        public void UseInteractiveProps()
        {
            if(!Available()) return;
            Collider2D[] collidersInCircle = Physics2D.OverlapCircleAll(gameObject.transform.position, _radius);

            for(int i = 0; i != collidersInCircle.Length; i++)
            {
                if(collidersInCircle[i].GetComponent<PropsImpact.IUsableProp>() == null) continue;

                collidersInCircle[i].GetComponent<PropsImpact.IUsableProp>().Use();

            }

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}