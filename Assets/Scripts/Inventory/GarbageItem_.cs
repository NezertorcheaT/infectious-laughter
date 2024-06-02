using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageItem_ : MonoBehaviour
{
[Min(1)]public int Level;
[Space(10f)]
[SerializeField] private GameObject KeyCodeTablet;

public void Suicide()
{
    Destroy(gameObject);
}

private void OnTriggerEnter2D(Collider2D other)
{
    if(other.gameObject.GetComponent<Entity.Abilities.GarbageAbility>())
    {
        KeyCodeTablet.SetActive(true);
    }
}

private void OnTriggerExit2D(Collider2D other)
{
    if(other.gameObject.GetComponent<Entity.Abilities.GarbageAbility>())
    {
        KeyCodeTablet.SetActive(false);
    }
}
}
