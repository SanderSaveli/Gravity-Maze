using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

[CustomEditor(typeof(SpriteShapeHeightApplier))]
public class SpriteShapeHeightApplierEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var applier = (SpriteShapeHeightApplier)target;

        GUILayout.Space(8);

        if (GUILayout.Button("Apply Height"))
        {
            foreach (var controller in applier.GetComponentsInChildren<SpriteShapeController>(true))
            {
                Undo.RecordObject(controller, "Apply SpriteShape Height");

                var spline = controller.spline;

                for (int i = 0; i < spline.GetPointCount(); i++)
                {
                    spline.SetHeight(i, applier.Height);
                }

                EditorUtility.SetDirty(controller);
            }

            foreach (var edge in applier.GetComponentsInChildren<EdgeCollider2D>(true))
            {
                Undo.RecordObject(edge, "Apply Edge Radius");

                edge.edgeRadius = applier.Height * 0.5f;

                EditorUtility.SetDirty(edge);
            }

            SceneView.RepaintAll();
        }
    }
}