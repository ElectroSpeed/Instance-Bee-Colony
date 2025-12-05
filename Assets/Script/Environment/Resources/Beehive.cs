using System;
using System.Collections.Generic;
using UnityEngine;

public class Beehive : MonoBehaviour {
    [Header("Stock")]
    [SerializeField] private int _pollenStock;
    [SerializeField] private int _honeyStock;
    private int _honeyToWaxCounter;

    [Header("Production Rates")]
    [SerializeField] private int _pollenToHoneyRate = 5;
    [SerializeField] private int _honeyToWaxRate = 5;
    
    private WaxManager _waxManager;
    
    public event Action<int> OnPollenChanged;
    public event Action<int> OnHoneyChanged;

    private void Awake()
    {
        OnPollenChanged?.Invoke(_pollenStock);
        OnHoneyChanged?.Invoke(_honeyStock);
    }

    public void AddPollen(int amount)
    {
        _pollenStock += amount;
        OnPollenChanged?.Invoke(_pollenStock);
        TryProduceHoney();
    }

    private void TryProduceHoney()
    {
        bool honeyProduced = false;

        if (_pollenStock >= _pollenToHoneyRate)
        {
            _pollenStock -= _pollenToHoneyRate;
            _honeyStock++;
            _honeyToWaxCounter++;
            honeyProduced = true;
        }

        if (!honeyProduced)
        {
            return;
        }
        
        OnPollenChanged?.Invoke(_pollenStock);
        OnHoneyChanged?.Invoke(_honeyStock);
        
        TryProduceWax();
    }

    private void TryProduceWax()
    {
        if (_honeyToWaxCounter >= _honeyToWaxRate)
        {
            _honeyToWaxCounter -= _honeyToWaxRate;
            _waxManager.AddWax(1);
        }
    }
    
    public int GetPollenStock() => _pollenStock;
    public int GetHoneyStock() => _honeyStock;
}