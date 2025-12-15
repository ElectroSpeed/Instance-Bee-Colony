using UnityEngine;

[CreateAssetMenu(fileName = "ContinuousPowerUtilisation", menuName = "GodGame/Power Utilisation/Continuous")]
public class ContinuousPowerUtilisation : SO_PowerUtilisation
{
    public override bool IsContinuous => true;

    public override void StartUse(PowerBase powerBase) => powerBase.StartUse();
    public override void UpdateUse(PowerBase powerBase) => powerBase.UpdateContinuous();
    public override void StopUse(PowerBase powerBase) { }
}