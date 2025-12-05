using System;
using UnityEngine;

public class WaxManager : MonoBehaviour
{
    [Header("Stock")]
    [SerializeField] private int _waxStock;

    public event Action<int> OnWaxChanged;

    public void AddWax(int amount)
    {
        _waxStock += amount;
        OnWaxChanged?.Invoke(_waxStock);
    }
    
    public void RemoveWax(int amount)
    {
        _waxStock -= amount;
        OnWaxChanged?.Invoke(_waxStock);
    }

    public int GetWaxStock()
    {
        return _waxStock;
    }

}
