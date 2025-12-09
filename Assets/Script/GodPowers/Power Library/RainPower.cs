public class RainPower : PowerBase
{
    private void Update()
    {
        if (!_usePower)
        {
            return;
        }
        ActivatePower();
    }
}