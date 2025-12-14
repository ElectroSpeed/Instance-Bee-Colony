using System.Collections.Generic;
using UnityEngine;

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
        IEnumerable<ITarget> targets = _usedPower._targetingPrefab.GetTargets();
        foreach (SO_EffectBehaviour effect in _usedPower._effectPrefabs)
        {
            effect?.ApplyEffect(targets);
        }
    }

    private void PlaySound()
    {
        AudioManager.Instance.PlaySFX(_usedPower._sound);
    }
}