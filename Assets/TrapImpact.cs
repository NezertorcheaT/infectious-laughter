using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapImpact : MonoBehaviour
{
    [SerializeField] private float StunTime;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Entity.Controllers.ControllerAI>())
        {
            other.gameObject.GetComponent<Entity.Abilities.EntityMovementHorizontalMove>().CanWalk = false;
            StartCoroutine(ReturnCanWalk(other));
        }
    }

    private IEnumerator ReturnCanWalk(Collider2D other)
    {
        yield return new WaitForSeconds(StunTime);
        Destroy(gameObject);
        other.gameObject.GetComponent<Entity.Abilities.EntityMovementHorizontalMove>().CanWalk = true;
    }
}
