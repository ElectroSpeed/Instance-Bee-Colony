[System.Serializable]
public class Power
{
    public string _name;
    public float _cooldown;
    public SO_TargetingBehaviour _targetingPrefab;
    public SO_EffectBehaviour[] _effectPrefabs;
    public SO_PowerUtilisation _utilisationMethod;
    public SoundType _sound;
}