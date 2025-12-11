using System;
using UnityEngine;

public class SeasonManager : MonoBehaviour
{
    //import enum
    public Season currentSeason;

    [SerializeField] public float seasonDuration; // Duration of each season in seconds
    private float seasonTimer;

    public event Action<Season> OnSeasonChanged;
    public event Action<Season> OnSeasonForcedChange;

    private void Start()
    {
        seasonTimer = seasonDuration;
        currentSeason = Season.Spring; // Start with Spring
        OnSeasonChanged?.Invoke(currentSeason);
    }

    private void Update()
    {
        seasonTimer -= Time.deltaTime;

        if (seasonTimer <= 0f)
        {
            AdvanceSeason();
            seasonTimer = seasonDuration;
        }
    }

    private void AdvanceSeason()
    {
        currentSeason = (Season)(((int)currentSeason + 1) % 4);
        OnSeasonChanged?.Invoke(currentSeason);
    }

    public void ForceNextSeason()
    {
        currentSeason = (Season)(((int)currentSeason + 1) % 4);
        OnSeasonForcedChange?.Invoke(currentSeason);
        seasonTimer = seasonDuration;
    }
}