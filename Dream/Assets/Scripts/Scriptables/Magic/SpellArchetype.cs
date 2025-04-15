using Magic;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellParameter
{
    public string name;
    public AnimationCurve curve;
    public Vector2 valueRange; // Min/Max output values
    public bool inverseWith; // Parameter to inversely affect
}

[CreateAssetMenu(fileName = "SpellArchetype", menuName = "Scriptable Objects/Magic/Spells/SpellArchetype")]
public class SpellArchetype : ScriptableObject
{
    public MagicType magicType;
    public List<SpellParameter> parameters = new List<SpellParameter>
    {
        new SpellParameter { name = "Damage", curve = AnimationCurve.Linear(0,0,1,1) },
        new SpellParameter { name = "ManaCost", curve = AnimationCurve.Linear(0,1,1,0) },
        new SpellParameter { name = "CastTime", curve = AnimationCurve.EaseInOut(0,1,1,0.5f) }
    };
}