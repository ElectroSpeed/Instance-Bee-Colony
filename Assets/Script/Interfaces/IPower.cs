using System.Collections.Generic;
using UnityEngine;

public interface IPower
{
    void SwitchActivationPower();
    void ActivatePower();
    bool CanActivatePower();
}