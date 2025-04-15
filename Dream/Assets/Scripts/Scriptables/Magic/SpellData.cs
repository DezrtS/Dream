using Magic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "Scriptable Objects/SpellData")]
public class SpellData : ScriptableObject
{
    [SerializeField] private MagicType magicType;
    [SerializeField] private MagicTier magicTier;
    [SerializeField] private string spellName;
    [SerializeField] private string spellDescription;
    [SerializeField] private EffectData[] effects;
    //[SerializeField] private Costs[] spellCosts;
    [SerializeField] private float castDuration;
    [SerializeField] private float cooldownDuration;

    public MagicType MagicType => magicType;
    public MagicTier MagicTier => magicTier;
    public string SpellName => spellName;
    public string SpellDescription => spellDescription;
    public EffectData[] Effects => effects;
    // public Costs[] SpellCosts => spellCosts;
    public float CastDuration => castDuration;
    public float CooldownDuration => cooldownDuration;

    public Spell AttachSpell(GameObject spellHolder)
    {
        Spell spell = spellHolder.AddComponent<Spell>();
        spell.Initialize(this);
        return spell;
    }
}