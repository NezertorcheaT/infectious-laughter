using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Entity.Abilities
{
[AddComponentMenu("Entity/Abilities/Dash Ability")]
    public class DashAbility : Ability
    {
        private Rigidbody2D selfRigidbody;
        [SerializeField] private EntityMovementHorizontalMove playerMovement;
        [SerializeField] private float dashForce;
        [SerializeField] private float dashCooldown;
        [SerializeField] private int dashCount;
        private int dashCountActive;
        public bool canDash = true;
        
        private void Start()
        {
            selfRigidbody = gameObject.GetComponent<Rigidbody2D>();
            dashCountActive = dashCount;
        }
        
        public void Dash()
        {
            if (dashCountActive <= 0) return;
            if (!canDash) return;
            
            if (playerMovement.RightTurn)
            {
                selfRigidbody.velocity = new Vector2(dashForce, 0);
            }
            else
            {
                selfRigidbody.velocity = new Vector2(-dashForce, 0);
            }

            dashCountActive--;
            Debug.Log(playerMovement.RightTurn);
            StartCoroutine(DashCooldownEnd());
        }

        private IEnumerator DashCooldownEnd()
        {
            yield return new WaitForSeconds(dashCooldown * dashCount);
            dashCountActive = dashCount;
        }
        
    }
}
