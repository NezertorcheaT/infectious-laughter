using Entity.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageManager : MonoBehaviour
{
    public static GarbageManager Instance;
    public event EventHandler OnBalanceChanged;

    private int _garbageBalance = 100;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnBalanceChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddGarbageBalance(int garbageAmount)
    {
        _garbageBalance += garbageAmount;
        OnBalanceChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SpentGarbageBalance(int garbageAmount)
    {
        _garbageBalance -= garbageAmount;
        OnBalanceChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IfCanAfford(int garbageAmount)
    {
        return _garbageBalance >= garbageAmount;
    }

    public int GetGarbageBalance()
    {
        return _garbageBalance;
    }

}
