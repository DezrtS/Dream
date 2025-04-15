using Effects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EffectActivator
{
    private Effect[] effects;

    public EffectActivator(IEnumerable<Effect> effects)
    {
        this.effects = effects.ToArray();
    }

    public virtual void Update(float deltaTime) { }
}