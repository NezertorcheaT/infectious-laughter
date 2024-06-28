using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuillotineImpact : MonoBehaviour
{
    [SerializeField] private float _maxYPosition;
    [SerializeField] private float _radius;
    [SerializeField] private Entity.Entity entity;
    [SerializeField] private float _levitationTime;

    private bool _used;

    private void Update()
    {
        if (gameObject.transform.position.y < _maxYPosition && !_used) StartCoroutine(Impact());
    }

    private IEnumerator Impact()
    {
        _used = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, _radius);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        
        for (int i = 0; i != colliders.Length; i++)
        {
            if(colliders[i].gameObject.GetComponent<Rigidbody2D>() &&
               !colliders[i].gameObject.GetComponent<Entity.Controllers.ControllerInput>())
            {
                colliders[i].gameObject.GetComponent<Rigidbody2D>().gravityScale = -0.02f;
            }
        }
        
        yield return new WaitForSeconds(_levitationTime);
        
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
