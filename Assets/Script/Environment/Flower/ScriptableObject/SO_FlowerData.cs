using UnityEngine;

[CreateAssetMenu(fileName = "NewFlower", menuName = "GodGame/Flower Data")]
public class SO_FlowerData : ScriptableObject
{
    [Header("Flower Description")]
    [SerializeField] private string _flowerName;
    [SerializeField] private string _description;
    [SerializeField] private GameObject _flowerPrefab;

    [Header("Flower Information")]
    [SerializeField] private float _lifeDuration;
    [SerializeField] private int _pollenAmount;
    [SerializeField] private float _growthDuration;
    [SerializeField] private AnimationCurve _growthCurve;
    [SerializeField] private GameObject _flowerButtonPrefab;

    [Header("Grow Conditions")]
    [Range(0, 100)] [SerializeField] private float _minHumidity;
    [Range(0, 100)] [SerializeField] private float _maxHumidity;
    [Range(0, 100)] [SerializeField] private float _minSunlight;
    [Range(0, 100)] [SerializeField] private float _maxSunlight;
    [Range(-10, 40)] [SerializeField] private float _minTemperature;
    [Range(-10, 40)] [SerializeField] private float _maxTemperature;
    

    public string FlowerName => _flowerName;
    public string Description => _description;
    public GameObject FlowerPrefab => _flowerPrefab;
    public GameObject FlowerButtonPrefab => _flowerButtonPrefab;

    public float LifeDuration => _lifeDuration;
    public int PollenAmount => _pollenAmount;
    public float GrowthDuration => _growthDuration;
    public AnimationCurve GrowthCurve => _growthCurve;

    public float MinHumidity => _minHumidity;
    public float MaxHumidity => _maxHumidity;
    public float MinSunlight => _minSunlight;
    public float MaxSunlight => _maxSunlight;
    public float MinTemperature => _minTemperature;
    public float MaxTemperature => _maxTemperature;
}