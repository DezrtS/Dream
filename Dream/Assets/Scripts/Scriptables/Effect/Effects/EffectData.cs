using Effects;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectData : ScriptableObject
{
    [SerializeField] private UseType effectUseType;
    [SerializeField] private ActivationType effectActivationType;
    [SerializeField] private ActivationType effectDeactivationType;
    [SerializeField] private List<EffectConditionData> effectConditions;

    public UseType EffectUseType => effectUseType;
    public ActivationType EffectActivationType => effectActivationType;
    public ActivationType EffectDeactivationType => effectDeactivationType;
    public List<EffectConditionData> EffectConditions => effectConditions;

    public ConditionResult CheckConditions(Context context)
    {
        foreach (EffectConditionData condition in effectConditions)
        {
            ConditionResult result = condition.CheckCondition(this, context);
            if (!result.Result)
            {
                Debug.Log($"Effect {name} failed condition {condition.name} [{condition.ConditionMessage}]");
                return new ConditionResult { Result = false, ConditionData = result.ConditionData };
            }
        }
        return new ConditionResult { Result = true, ConditionData = null };
    }

    public ConditionResult CheckConditionsOnActivationEvent(Context context, ActivationType activationType)
    {
        if (activationType == effectActivationType || activationType == ActivationType.OnActivate)
        {
            return CheckConditions(context);
        }

        return new ConditionResult { Result = true, ConditionData = null };
    }

    public abstract void Randomize(SpellData spellData);
    public abstract Effect AttachEffect(GameObject effectHolder);
}