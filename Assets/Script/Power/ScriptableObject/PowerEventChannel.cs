using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Power Event Channel")]
public class PowerEventChannel : ScriptableObject
{
    public Action<PowerType> OnPowerSelected;
}