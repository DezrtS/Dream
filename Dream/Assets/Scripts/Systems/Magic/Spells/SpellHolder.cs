using System.Collections.Generic;
using UnityEngine;

public class SpellHolder : MonoBehaviour
{
    [SerializeField] private int spellCapacity = 5;

    private GameObject spellHolder;
    private SpellData[] spellDatas;
    private Dictionary<SpellData, Spell> primarySpells;
    private List<Spell> secondarySpells;

    private int activeSpellIndex = 0;

    public SpellData[] SpellDatas => spellDatas;
    public Dictionary<SpellData, Spell> PrimarySpells => primarySpells;
    public List<Spell> SecondarySpells => secondarySpells;
    public int ActiveSpellIndex { get => activeSpellIndex; set { activeSpellIndex = (value + spellCapacity) % spellCapacity; } }

    private void Awake()
    {
        spellHolder = new GameObject("SpellHolder");
        spellHolder.transform.parent = transform;

        spellDatas = new SpellData[spellCapacity];
        primarySpells = new Dictionary<SpellData, Spell>();
        secondarySpells = new List<Spell>();
    }

    public void AddSpell(SpellData spellData)
    {
        for (int i = 0; i < spellCapacity; i++)
        {
            if (spellDatas[i] == null)
            {
                spellDatas[i] = spellData;
                Spell spell = spellData.AttachSpell(spellHolder);
                primarySpells.Add(spellData, spell);
                return;
            }
        }
    }

    public SpellData GetSpellData()
    {
        return GetSpellData(activeSpellIndex);
    }

    public SpellData GetSpellData(int index)
    {
        return spellDatas[index];
    }

    public Spell GetSpell()
    {
        return GetSpell(GetSpellData(activeSpellIndex));
    }

    public Spell GetSpell(SpellData spellData)
    {
        primarySpells.TryGetValue(spellData, out Spell spell);
        return spell;
    }
}