using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HomingDartPrefab : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float stunTime;
    public float speed;
    private Transform _selfTransform;
    private Transform _target;
    private Entity.Abilities.Stun _stun;
    

    private void Start()
    {
        _selfTransform = gameObject.transform;
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
            _selfTransform.rotation = Quaternion.Euler(_selfTransform.rotation.eulerAngles.x, _selfTransform.rotation.eulerAngles.y, Mathf.Atan2(_target.position.y - _selfTransform.position.y, _target.position.x - _selfTransform.position.x) * Mathf.Rad2Deg - 90);
            _selfTransform.position = Vector3.Lerp(_selfTransform.position, _target.position, (speed / 1000) / Vector3.Distance(_selfTransform.position, _target.position));
            if(Vector3.Distance(_selfTransform.position, _target.position) < 0.05) Die();
            yield return null;
        }
    }

    private void Die()
    {
        _stun.Perform(stunTime);
        Destroy(gameObject);
    }
}
