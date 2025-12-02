using System.Collections.Generic;

public interface IEffect
{
    void ApplyEffect(IEnumerable<ITarget> targets);
}