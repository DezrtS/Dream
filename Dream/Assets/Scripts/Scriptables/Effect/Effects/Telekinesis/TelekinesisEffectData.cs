using Effects;
using UnityEngine;

[CreateAssetMenu(fileName = "TelekinesisEffectData", menuName = "Scriptable Objects/Effects/TelekinesisEffectData")]
public class TelekinesisEffectData : EffectData
{
    [SerializeField] private AnimationCurve animationCurve;

    [Header("Movement Settings")]
    [SerializeField] private float strength;
    [SerializeField] private float dampingStrength;
    [SerializeField] private float distance;

    [Header("Advanced Settings")]
    [Tooltip("Distance at which force starts reducing")]
    [SerializeField] private float slowdownDistance;

    [Header("Stabilization")]
    [SerializeField] private bool stabilizeHeight;
    [SerializeField] private float liftFactor;

    public float Strength => strength;
    public float DampingStrength => dampingStrength;
    public float Distance => distance;

    public float SlowdownDistance => slowdownDistance;

    public bool StabilizeHeight => stabilizeHeight;
    public float LiftFactor => liftFactor;

    public override Effect AttachEffect(GameObject effectHolder)
    {
        TelekinesisEffect telekinesisEffect = effectHolder.AddComponent<TelekinesisEffect>();
        telekinesisEffect.Initialize(this);
        return telekinesisEffect;
    }

    public override void Randomize(SpellData spellData)
    {
        throw new System.NotImplementedException();
    }
}