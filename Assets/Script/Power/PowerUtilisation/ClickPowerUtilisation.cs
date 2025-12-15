using UnityEngine;

[CreateAssetMenu(fileName = "ClickPowerUtilisation", menuName = "GodGame/Power Utilisation/Click")]
public class ClickPowerUtilisation : SO_PowerUtilisation
{
    public override bool IsContinuous => false;

    public override void StartUse(PowerBase powerBase) => powerBase.StartUse();
    public override void UpdateUse(PowerBase powerBase) { }
    public override void StopUse(PowerBase powerBase) { }
}