using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(SpellArchetype))]
public class SpellArchetypeEditor : Editor
{
    private const int TIER_STEPS = 10;
    private Vector2 scrollPos;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        var archetype = target as SpellArchetype;
        if(archetype == null) return;

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Tier Visualization", EditorStyles.boldLabel);
        
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(300));
        
        // Draw curve matrix
        foreach(var param in archetype.parameters)
        {
            EditorGUILayout.CurveField(param.curve, Color.red, 
                new Rect(0, param.valueRange.x, 1, param.valueRange.y - param.valueRange.x), 
                GUILayout.Height(80));
        }
        
        EditorGUILayout.EndScrollView();

        // Tier preview table
        EditorGUILayout.LabelField("Tier Value Preview");
        EditorGUILayout.BeginHorizontal();
        for(int i=0; i<TIER_STEPS; i++)
        {
            float tierProgress = i / (float)TIER_STEPS;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField($"T{(i+1)}");
            foreach(var param in archetype.parameters)
            {
                float value = Mathf.Lerp(
                    param.valueRange.x, 
                    param.valueRange.y, 
                    param.curve.Evaluate(tierProgress)
                );
                EditorGUILayout.LabelField($"{value:0.#}");
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif