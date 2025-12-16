using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;
using UnityEngine.XR;

public class SeasonUI : MonoBehaviour
{
    [SerializeField] private GameObject _seasonWheelUI;

    [SerializeField] SeasonManager _seasonManager;

    private void OnEnable()
    {
        _seasonManager.OnSeasonChanged += HandleSeasonChanged;
        _seasonManager.OnSeasonForcedChange += HandleSeasonForcedChange;
    }

    private void HandleSeasonChanged(Season season)
    {       
        float delay = _seasonManager._seasonDuration;
        
        switch (season)
        {
            case Season.Spring:  
                RotateWheel(46, delay, false);
                break;
            case Season.Summer:
                RotateWheel(136, delay, false);
                break;
            case Season.Autumn:
                RotateWheel(226, delay, false);
                break;
            case Season.Winter:
                RotateWheel(316, delay, false);
                break;
        }
    }

    private void HandleSeasonForcedChange(Season season)
    {
        float delay = 0.5f;
        switch (season)
        {
            case Season.Spring:
                RotateWheel(0, delay, true);
                break;
            case Season.Summer:
                RotateWheel(90, delay, true);
                break;
            case Season.Autumn:
                RotateWheel(180, delay, true);
                break;
            case Season.Winter:
                RotateWheel(270, delay, true);
                HandleSeasonChanged(season);
                break;
        }

    }

    private void RotateWheel(float targetAngle, float duration, bool isEase)
    {
        StopAllCoroutines();
        StartCoroutine(RotateWheelSmooth(targetAngle, duration, isEase));
    }

private IEnumerator RotateWheelSmooth(float targetAngle, float duration, bool isEase)
{
    float time = 0f;

    Quaternion start = _seasonWheelUI.transform.localRotation;
    Quaternion end = Quaternion.AngleAxis(targetAngle, Vector3.forward);

    while (time < duration)
    {
        time += Time.deltaTime;
        float t = time / duration;

        if (isEase)   t = 1f - Mathf.Pow(1f - t, 3);
        // cubic ease-out
      

        _seasonWheelUI.transform.localRotation = Quaternion.Lerp(start, end, t);

        yield return null;
    }

    _seasonWheelUI.transform.localRotation = end;
}


}