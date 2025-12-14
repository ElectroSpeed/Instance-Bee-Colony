using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerLibrary", menuName = "GodGame/Power Library")]
public class PowerLibrary : ScriptableObject
{
    public List<Power> _powers = new();
    
    public Power GetPower(PowerType type)
    {
        string powerName = type.ToString();

        foreach (Power power in _powers)
            if (power._name == powerName)
                return power;
        Debug.LogWarning($"Pouvoir {powerName} introuvable.");
        return null;
    }
}