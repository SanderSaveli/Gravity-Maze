using UnityEditor;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [CustomEditor(typeof(PressMover))]
    public class EditorPressMover : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            GUILayout.Label("Record Positions", EditorStyles.boldLabel);

            PressMover mover = (PressMover)target;

            if (GUILayout.Button("Record Point 1"))
            {
                Undo.RecordObject(mover, "Record Point 1");
                mover.RecordPoint1();
                EditorUtility.SetDirty(mover);
            }

            if (GUILayout.Button("Record Point 2"))
            {
                Undo.RecordObject(mover, "Record Point 2");
                mover.RecordPoint2();
                EditorUtility.SetDirty(mover);
            }
        }
    }
}
