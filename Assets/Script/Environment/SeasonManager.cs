using System;
using UnityEngine;

public class SeasonManager : MonoBehaviour
{
    [HideInInspector] public Season _currentSeason;
    public float _seasonDuration;
    
    private float _seasonTimer;

    public event Action<Season> OnSeasonChanged;
    public event Action<Season> OnSeasonForcedChange;

    private void Start()
    {
        _seasonTimer = _seasonDuration;
        _currentSeason = Season.Spring;
        OnSeasonChanged?.Invoke(_currentSeason);
    }

    private void Update()
    {
        _seasonTimer -= Time.deltaTime;

        if (_seasonTimer <= 0f)
        {
            ChangeSeason();
            _seasonTimer = _seasonDuration;
        }
    }

    private void ChangeSeason()
    {
        _currentSeason = (Season)(((int)_currentSeason + 1) % 4);
        OnSeasonChanged?.Invoke(_currentSeason);
    }

    public void GoToNextSeason()
    {
        ChangeSeason();
        _seasonTimer = _seasonDuration;
    }
}