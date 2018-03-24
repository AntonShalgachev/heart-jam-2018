using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField]
    private int startingBalance;

    private int balance;
    private float partialBalance = 0.0f;

    public event Action<int> OnCurrencyChanged;

    private void Start()
    {
        balance = startingBalance;

        UpdateBalance(0);
    }

    private void UpdateBalance(int delta)
    {
        balance += delta;

        if (OnCurrencyChanged != null)
            OnCurrencyChanged(balance);
    }

    public bool HasFunds(int amount)
    {
        return balance >= amount;
    }

    public void Gain(int amount)
    {
        Debug.Assert(amount >= 0, "amount should not be negative");

        UpdateBalance(amount);
    }

    public void Gain(float amount)
    {
        Debug.Assert(amount >= 0, "amount should not be negative");

        var wholeAmount = (int)amount;
        var partialAmount = amount - wholeAmount;

        Debug.Assert(partialAmount >= 0.0f && partialAmount < 1.0f);

        Gain(wholeAmount);
        partialBalance += partialAmount;

        Debug.Assert(partialBalance >= 0.0f && partialBalance < 2.0f);

        if (partialBalance > 1.0f)
        {
            Gain(1);
            partialBalance -= 1.0f;
        }

        Debug.Assert(partialBalance >= 0.0f && partialBalance < 1.0f);
    }

    public bool Purchase(int amount)
    {
        if (HasFunds(amount))
        {
            UpdateBalance(-amount);
            return true;
        }

        return false;
    }
}
