using Effects;
using UnityEngine;

public abstract class EffectConditionData : ScriptableObject
{
    [SerializeField] private string conditionMessage;
    public string ConditionMessage => conditionMessage;
    public abstract ConditionResult CheckCondition(EffectData effectData, Context effectContext);
}