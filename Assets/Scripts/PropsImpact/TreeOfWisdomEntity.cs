using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Installers;


public class TreeOfWisdomEntity : MonoBehaviour
{
    private bool _used = false;
    private Entity.Abilities.EntityHp _hpAbility;
    [Inject] private PlayerInstallation _playerInstallation;

    private void Start()
    {
        _hpAbility = _playerInstallation.Entity.GetComponent<Entity.Abilities.EntityHp>();
    }

    public void UseTreeOfWisdom()
    {
        Debug.Log("lololo");
        if(_used) return;
        _hpAbility.Heal( _hpAbility.MaxHp - _hpAbility.Hp);
        _used = true;
    }
}
