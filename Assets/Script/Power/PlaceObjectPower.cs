using UnityEngine.InputSystem;
using System.Diagnostics;

public class PlaceObjectPower : PowerBase
{
    public void UsePower(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Print("input");
            ActivatePower();
        }
    }
}