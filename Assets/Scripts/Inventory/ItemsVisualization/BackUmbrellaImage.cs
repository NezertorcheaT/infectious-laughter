using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Installers;

public class BackUmbrellaImage : MonoBehaviour
{

    [SerializeField] private SpriteRenderer backSpriteRenderer;
    [SerializeField] private Entity.Abilities.LightResponsive playerLightResponse;
    [SerializeField] private Entity.Abilities.HorizontalMovement playerHorizontalMovement;

    private void OnEnable()
    {
        playerHorizontalMovement.OnTurn += CheckTurn;
        playerLightResponse.OnChangeResistance += CheckSelfActive;
    }

    private void OnDisable()
    {
        playerHorizontalMovement.OnTurn -= CheckTurn;
        playerLightResponse.OnChangeResistance -= CheckSelfActive;
    }


    private void CheckTurn(bool turn) => backSpriteRenderer.flipX = turn;

    private void CheckSelfActive(bool activeStatus) => backSpriteRenderer.gameObject.SetActive(activeStatus);
}
