using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HomingDartPrefab : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float stunTime;
    [SerializeField] private float speed;
    private Transform _target;
    private Entity.Abilities.Stun _stun;
    

    private void Start()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(gameObject.transform.position, radius);


        for(int i = 0; i != hits.Length; i++)
        {
            if(hits[i].gameObject.GetComponent<Entity.Controllers.ControllerAI>())
            {
                _target = hits[i].gameObject.transform;
                _stun = _target.gameObject.GetComponent<Entity.Abilities.Stun>();
                StartCoroutine(GoToTarget());
                return;
            }
        }
    }

    private IEnumerator GoToTarget()
    {
        while(true)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(_target.position.y - transform.position.y, _target.position.x - transform.position.x) * Mathf.Rad2Deg - 90);
            transform.position = Vector3.Lerp(transform.position, _target.position, (speed / 1000) / Vector3.Distance(transform.position, _target.position));
            if(Vector3.Distance(transform.position, _target.position) < 0.05) Die();
            yield return null;
        }
    }

    private void Die()
    {
        _stun.Perform(stunTime);
        Destroy(gameObject);
    }
}
