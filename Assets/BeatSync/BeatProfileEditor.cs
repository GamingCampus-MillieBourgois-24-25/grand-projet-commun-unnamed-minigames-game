using Assets._Common.Scripts;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BeatProfile))]
public class BeatProfileEditor : Editor
{
    private BeatProfile profile;
    private SerializedProperty beatsProperty;

    private void OnEnable()
    {
        profile = (BeatProfile)target;
        beatsProperty = serializedObject.FindProperty((nameof(BeatProfile.beats)));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        bool changed = false;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Beats", EditorStyles.boldLabel);
        if (beatsProperty == null)
        {
            EditorGUILayout.HelpBox("Propriété 'beats' introuvable. Assurez-vous que 'Beat' est une classe [Serializable].", MessageType.Error);
            return;
        }

        for (int i = 0; i < beatsProperty.arraySize; i++)
        {
            var beatProp = beatsProperty.GetArrayElementAtIndex(i);
            var typeProp = beatProp.FindPropertyRelative("type");

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(typeProp, new GUIContent($"Beat {i + 1}"));
            if (EditorGUI.EndChangeCheck())
            {
                changed = true;
            }
        }

        if (changed)
        {
            profile.isDirty = true;
        }

        if (profile.isDirty)
        {
            EditorGUILayout.HelpBox("Les durées ne sont plus à jour. Cliquez sur le bouton pour les recalculer.", MessageType.Warning);
        }

        if (GUILayout.Button("Recalculer les durées"))
        {
            RecalculateDurations();
            profile.isDirty = false;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void RecalculateDurations()
    {
        float cumulativeTime = 0f;

        for (int i = 0; i < profile.beats.Count; i++)
        {
            var b = profile.beats[i];
            b.duration = GetDurationForType(b.type, profile.baseTempo);
            cumulativeTime += b.duration;
            b.duration = cumulativeTime; // Update duration to be cumulative
            profile.beats[i] = b;
        }

        EditorUtility.SetDirty(profile);
    }

    private float GetDurationForType(BeatType type, float tempo)
    {
        // tempo = battements par minute, donc 60s / tempo = 1 noire
        float beatTime = 60f / tempo;

        return type switch
        {
            BeatType.Blanche => beatTime * 2f,
            BeatType.NoirePointée => beatTime * 1.5f,
            BeatType.Noire => beatTime,
            BeatType.CrochePointée => beatTime * 0.75f,
            BeatType.Croche => beatTime * 0.5f,
            BeatType.DoubleCroche => beatTime * 0.25f,
            _ => beatTime
        };
    }
}
