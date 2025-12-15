using System.Collections.Generic;
public class PowerBase
{
    public readonly Power _usedPower;
    public PowerBase(Power usedPower) => _usedPower = usedPower;
    
    public void StartUse()
    {
        ApplyEffects();
        PlaySound();
    }
    
    public void UpdateContinuous()
    {
        ApplyEffects();
    }
    
    private void ApplyEffects()
    {
        if (_usedPower._targetingPrefab != null)
        {
            IEnumerable<ITarget> targets = _usedPower._targetingPrefab.GetTargets();
            foreach (SO_EffectBehaviour effect in _usedPower._effectPrefabs)
            {
                effect?.ApplyEffect(targets);
            }
        }
        
        foreach (SO_EffectBehaviour effect in _usedPower._effectPrefabs)
        {
            effect?.ApplyEffect(null);
        }
    }

    private void PlaySound()
    {
        AudioManager.Instance.PlaySFX(_usedPower._sound);
    }
}