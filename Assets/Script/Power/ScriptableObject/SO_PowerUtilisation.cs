using UnityEngine;

public abstract class SO_PowerUtilisation : ScriptableObject
{
    public abstract bool IsContinuous { get; }

    public abstract void StartUse(PowerBase powerBase);
    public abstract void UpdateUse(PowerBase powerBase);
    public abstract void StopUse(PowerBase powerBase);
}