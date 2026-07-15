using System;
using UnityEngine;

public class CurrencyModel
{
    private int _amount;
    public int Amount
    {
        get => _amount;
        set => _amount = Mathf.Max(0, value);
    }
}
