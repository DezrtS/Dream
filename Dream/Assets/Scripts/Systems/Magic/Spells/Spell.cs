using Effects;
using UnityEngine;
using static IUseable;

public class Spell : MonoBehaviour, IUseable
{
    public event UseHandler OnUsed;
    public delegate void SpellHandler(Spell spell);
    public event SpellHandler OnSpellReady;
    public delegate void SpellActivateHandler(Spell spell, bool isActive);
    public event SpellActivateHandler OnSpellActivated;

    private bool isUsing;

    private SpellData spellData;
    private float castDurationTimer;
    private float cooldownUntil;

    private bool isReady;
    private bool isActive;

    private GameObject effectHolder;
    private Effect[] effects;
    private UseContext useContext;

    public bool IsUsing => isUsing;

    public SpellData SpellData => spellData;
    public float CooldownUntil => cooldownUntil;

    public bool IsReady => isReady;
    public bool IsActive => isActive;

    public void Initialize(SpellData spellData)
    {
        this.spellData = spellData;
        castDurationTimer = spellData.CastDuration;
        cooldownUntil = 0f;

        if (effectHolder != null) Destroy(effectHolder);
        effectHolder = new GameObject("EffectHolder");
        effectHolder.transform.parent = transform;

        effects = new Effect[spellData.Effects.Length];
        for (int i = 0; i < spellData.Effects.Length; i++)
        {
            effects[i] = spellData.Effects[i].AttachEffect(effectHolder);
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        UpdateTimers(deltaTime);
    }

    private void UpdateTimers(float deltaTime)
    {
        if (isUsing)
        {
            if (castDurationTimer > 0f)
            {
                castDurationTimer -= deltaTime;
                if (castDurationTimer <= 0f)
                {
                    castDurationTimer = 0f;
                    foreach (Effect effect in effects)
                    {
                        effect.UpdateAnimationProgress(1f);
                    }

                    isReady = true;
                    TriggerActivationEvent(ActivationType.OnReady);
                    OnSpellReady?.Invoke(this);
                }
                else
                {
                    foreach (Effect effect in effects)
                    {
                        effect.UpdateAnimationProgress(1f - (castDurationTimer / spellData.CastDuration));
                    }
                }
            }
        }
        else
        {
            if (castDurationTimer < spellData.CastDuration)
            {
                castDurationTimer += deltaTime;
                if (castDurationTimer >= spellData.CastDuration)
                {
                    castDurationTimer = spellData.CastDuration;
                    foreach (Effect effect in effects)
                    {
                        effect.UpdateAnimationProgress(0);
                    }
                }
                else
                {
                    foreach (Effect effect in effects)
                    {
                        effect.UpdateAnimationProgress(1f - (castDurationTimer / spellData.CastDuration));
                    }
                }
            }
        }
    }

    public bool CanUse()
    {
        return cooldownUntil <= Time.time && !isUsing;
    }

    public bool CanStopUsing()
    {
        return isUsing;
    }

    public void Use(UseContext useContext)
    {
        if (CanUse())
        {
            this.useContext = useContext;
            isUsing = true;
            OnUsed?.Invoke(this, isUsing, useContext);
            TriggerActivationEvent(ActivationType.OnUse);
            //if (isActive) DeactivateEffects();
        }
    }

    public void StopUsing(UseContext useContext)
    {
        if (CanStopUsing())
        {
            this.useContext = useContext;
            isUsing = false;
            OnUsed?.Invoke(this, isUsing, useContext);
            if (isReady) TriggerActivationEvent(ActivationType.OnStopUse);
            else TriggerActivationEvent(ActivationType.OnCancel);
            //if (isReady) ActivateEffects();
            isReady = false;
        }
    }

    public void TriggerActivationEvent(ActivationType activationType)
    {
        Context effectContext = Context.ConstructContext(SourceType.Magic, useContext, spellData, null);
        foreach (EffectData effect in spellData.Effects)
        {
            ConditionResult result = effect.CheckConditionsOnActivationEvent(effectContext, activationType);
            if (!result.Result)
            {
                Debug.Log($"Spell {spellData.SpellName} failed condition {result.ConditionData.ConditionMessage}");
                return;
            }
        }

        foreach (Effect effect in effects)
        {
            effect.HandleActivationEvent(effectContext, activationType);
        }
    }
}