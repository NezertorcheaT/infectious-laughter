using Entity.Abilities;
using UnityEngine;

public class TrapImpact : MonoBehaviour
{
    [SerializeField] private float stunTime = 5;

    private async void OnCollisionEnter2D(Collision2D other)
    {
        var ability = other.collider.gameObject.GetComponent<EntityStunAbility>();
        if (ability is null) return;
        GetComponent<Rigidbody2D>().simulated = false;
        await ability.Stun(stunTime);
        Destroy(gameObject);
    }
}