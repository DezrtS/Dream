using UnityEngine;

public class TestPlayer : Entity
{
    [SerializeField] private SpellHolder spellHolder;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            spellHolder.GetSpell().Use(new UseContext() { SourceEntity = this });
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            spellHolder.GetSpell().StopUsing(new UseContext() { SourceEntity = this });
        }
    }
}
