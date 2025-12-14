using UDK.ViewElements;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BarView<>), true)]
public class BarViewEditor : Editor
{
    SerializedProperty valueBar;
    SerializedProperty isAnimated;
    SerializedProperty animationDelay;
    SerializedProperty animationSpeed;
    SerializedProperty depletionIndicator;

    void OnEnable()
    {
        valueBar = serializedObject.FindProperty("valueBar");
        isAnimated = serializedObject.FindProperty("isAnimated");
        animationDelay = serializedObject.FindProperty("animationDelay");
        animationSpeed = serializedObject.FindProperty("animationSpeed");
        depletionIndicator = serializedObject.FindProperty("depletionIndicator");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Скройте поле Script
        // DrawScriptField();

        // Отобразите свойства в нужном порядке
        EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(valueBar);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Animation Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(isAnimated);

        if (isAnimated.boolValue)
        {
            EditorGUILayout.PropertyField(animationDelay);
            EditorGUILayout.PropertyField(animationSpeed);
            EditorGUILayout.PropertyField(depletionIndicator);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawScriptField()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true);
        EditorGUI.EndDisabledGroup();
    }
}
