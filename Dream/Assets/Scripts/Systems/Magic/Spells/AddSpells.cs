using System.Collections.Generic;
using UnityEngine;

public class AddSpells : MonoBehaviour
{
    [SerializeField] private List<SpellData> spellsToAdd;
    private SpellHolder spellHolder;

    void Start()
    {
        spellHolder = GetComponent<SpellHolder>();
        if (spellHolder == null)
        {
            Debug.LogError("SpellHolder component not found on this GameObject.");
            return;
        }

        foreach (SpellData spell in spellsToAdd)
        {
            spellHolder.AddSpell(spell);
        }
    }
}
