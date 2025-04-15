using Effects;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolEffectConditionData", menuName = "Scriptable Objects/Effect Conditions/BoolEffectConditionData")]
public class BoolEffectConditionData : EffectConditionData
{
    [SerializeField] private bool condition;
    public override ConditionResult CheckCondition(EffectData effectData, Context effectContext)
    {
        return new ConditionResult { Result = condition, ConditionData = this };
    }
}
