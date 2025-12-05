using UnityEngine;

[CreateAssetMenu(fileName = "NewFlower", menuName = "GodGame/Flower Data")]
public class SO_FlowerData : ScriptableObject
{
    public string _flowerName;
    public string _description;
    public AnimationCurve _growthCurve;
    public float _growthDuration;
    public float _lifeDuration;
    public int _pollenAmount;
    public GameObject _flowerPrefab;

    [Header("Conditions environnementales")]
    [Range(0, 100)] public float _minHumidity;
    [Range(0, 100)] public float _maxHumidity;
    [Range(0, 100)] public float _minSunlight;
    [Range(0, 100)] public float _maxSunlight;
    [Range(-40, 40)] public float _minTemperature;
    [Range(-40, 40)] public float _maxTemperature;
}