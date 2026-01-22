using UnityEditor;
using UnityEngine;
using SanderSaveli.GravityMaze;

[CustomEditor(typeof(GameButtonTransformHandler))]
public class GameButtonTransformHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Record"))
        {
            GameButtonTransformHandler handler = (GameButtonTransformHandler)target;
            Undo.RecordObject(handler, "Record Transform");
            handler.Record();
            EditorUtility.SetDirty(handler);
        }
    }
}
