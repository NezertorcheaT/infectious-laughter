using UnityEngine;

public class BackUmbrellaImage : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backSpriteRenderer;
    [SerializeField] private Entity.Abilities.LightResponsive playerLightResponse;

    private void OnEnable()
    {
        playerLightResponse.OnChangeResistance += CheckSelfActive;
    }

    private void OnDisable()
    {
        playerLightResponse.OnChangeResistance -= CheckSelfActive;
    }

    private void CheckSelfActive(bool activeStatus) => backSpriteRenderer.gameObject.SetActive(activeStatus);
}