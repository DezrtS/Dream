using Effects;
using UnityEngine;


[CreateAssetMenu(fileName = "HasTargetEffectConditionData", menuName = "Scriptable Objects/Effect Conditions/HasTargetCondition")]
public class HasTargetEffectConditionData : EffectConditionData
{
    public override ConditionResult CheckCondition(EffectData effectData, Context effectContext)
    {
        bool condition = effectContext.TargetEntity != null;
        return new ConditionResult { Result = condition, ConditionData = this };
    }
}