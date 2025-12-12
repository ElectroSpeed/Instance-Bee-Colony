using UnityEngine;
using System.Collections;

public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter
}

public class SeasonUI : MonoBehaviour
{
    [SerializeField] private Season _currentSeason;

    [SerializeField] private GameObject _seasonWheelUI;

    public void SetSeason()
    {
        switch (_currentSeason)
        {
            case Season.Spring:
                
                RotateWheel(0);
                break;
            case Season.Summer:
                RotateWheel(90);
                break;
            case Season.Autumn:
                RotateWheel(180);
                break;
            case Season.Winter:
                RotateWheel(270);
                break;
        }
    }

    public void RotateWheel(float targetAngle, float duration = 1f)
    {
        StartCoroutine(RotateWheelEaseOutCoroutine(targetAngle, duration));
    }

    private IEnumerator RotateWheelEaseOutCoroutine(float targetAngle, float duration)
    {
        float time = 0f;

        Quaternion startRot = _seasonWheelUI.transform.rotation;
        Quaternion endRot = Quaternion.Euler(0, 0, targetAngle);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            t = 1f - Mathf.Pow(1f - t, 3); // Ease out cubic
    

            _seasonWheelUI.transform.rotation = Quaternion.Lerp(startRot, endRot, t);

            yield return null;
        }

        _seasonWheelUI.transform.rotation = endRot;
    }

}