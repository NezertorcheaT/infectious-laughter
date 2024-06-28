using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuillotineImpact : MonoBehaviour
{
    public float maxYPosition;
    public float radius;
    public Entity.Entity entity;
    public float levitationTime;

    private bool _used;

    private void Update()
    {
        if (gameObject.transform.position.y < maxYPosition && !_used) StartCoroutine(Impact());
    }

    private IEnumerator Impact()
    {
        _used = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, radius);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        
        for (int i = 0; i != colliders.Length; i++)
        {
            if(colliders[i].gameObject.GetComponent<Rigidbody2D>() &&
               !colliders[i].gameObject.GetComponent<Entity.Controllers.ControllerInput>())
            {
                colliders[i].gameObject.GetComponent<Rigidbody2D>().gravityScale = -0.02f;
            }
        }
        
        yield return new WaitForSeconds(levitationTime);
        
        for (int i = 0; i != colliders.Length; i++)
        {
            if (colliders[i].gameObject.GetComponent<Rigidbody2D>())
            {
                colliders[i].gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
            }
        }
        Destroy(gameObject);
    }
}
