using UnityEngine;

[CreateAssetMenu(fileName = "NewFlower", menuName = "GodGame/Flower Data")]
public class SO_FlowerData : ScriptableObject
{
    public string _flowerName;
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
}