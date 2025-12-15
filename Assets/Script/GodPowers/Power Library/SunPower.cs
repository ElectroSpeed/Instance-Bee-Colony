public class SunPower : PowerBase
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