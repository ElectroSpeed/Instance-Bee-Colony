using System;
using System.Collections.Generic;
using UnityEngine;

public class Beehive : MonoBehaviour
{
    [Header("Stock")]
    [SerializeField] private int _pollenStock;
    [SerializeField] private int _honeyStock;
    private int _honeyToWaxCounter;

    [Header("Production Rates")]
    [SerializeField] private int _pollenToHoneyRate = 5;
    [SerializeField] private int _honeyToWaxRate = 5;

    [Header("Exploration Area")]
    public float explorationRadius = 25f;

    [Header("Bee Spawn")]
    [SerializeField] private GameObject beePrefab;
    [SerializeField] private Transform spawnPoint; // if nothing so spawn at beehive position
    [SerializeField] private int honeyPerBee = 3;

    private WaxManager _waxManager;

    public event Action<int> OnPollenChanged;
    public event Action<int> OnHoneyChanged;

    private void Awake()
    {
        OnPollenChanged?.Invoke(_pollenStock);
        OnHoneyChanged?.Invoke(_honeyStock);
    }

    private void Start()
    {
        SpawnBee();
    }

    public void AddPollen(int amount)
    {
        _pollenStock += amount;
        OnPollenChanged?.Invoke(_pollenStock);
        TryProduceHoney();
    }

    private void SpawnBee()
    {
        if (beePrefab == null)
        {
            Debug.LogError("Bee prefab is missing on Beehive");
            return;
        }

        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        Instantiate(beePrefab, pos, Quaternion.identity);
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

        TrySpawnBee();
        TryProduceWax();
    }

    private void TrySpawnBee()
    {
        if (_honeyStock < honeyPerBee)
            return;

        _honeyStock -= honeyPerBee;
        OnHoneyChanged?.Invoke(_honeyStock);

        SpawnBee();
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