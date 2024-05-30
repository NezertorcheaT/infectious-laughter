using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Entity.Abilities
{
public class PlayerJumpAbility : Ability
{


    private Rigidbody2D _playerRb;


    private int jumpCountActive;

    [Space(10.0f), SerializeField, Min(1)] private int jumpCount = 1;
    [SerializeField] private float jumpHeight = 3;
    private float groundDistance;


    private void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        jumpCountActive = jumpCount;;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(CheckGround())
        {
        jumpCountActive = jumpCount;
        }
    }
    public void Jump()
    {
        if(jumpCountActive != 0)
        {
        _playerRb.velocity = new Vector2(_playerRb.velocity.x, jumpHeight);
        jumpCountActive -= 1;
        }
    }
    private bool CheckGround()
    {
        if(Physics2D.Raycast(_playerRb.position, -_playerRb.transform.up, groundDistance))
        {
            return true;
        }else{
            return false;
        }

    }
    private void Update()
    {
        Debug.DrawRay(_playerRb.position, -_playerRb.transform.up * groundDistance, Color.yellow);
    }
}
}