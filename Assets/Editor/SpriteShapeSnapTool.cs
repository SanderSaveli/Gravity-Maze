using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.U2D;

[EditorTool("SpriteShape Snap Tool")]
public class SpriteShapeSnapTool : EditorTool
{
    private const float SNAP_STEP = 0.25f;

    public override void OnToolGUI(EditorWindow window)
    {
        var controller = Selection.activeGameObject?.GetComponent<SpriteShapeController>();
        if (controller == null)
            return;

        var spline = controller.spline;
        Event e = Event.current;

        for (int i = 0; i < spline.GetPointCount(); i++)
        {
            Vector3 worldPos = controller.transform.TransformPoint(spline.GetPosition(i));

            EditorGUI.BeginChangeCheck();
            Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(controller, "Move SpriteShape Point");

                Vector3 localPos = controller.transform.InverseTransformPoint(newWorldPos);

                if (e.control) // Ctrl зажат
                {
                    localPos.x = Mathf.Round(localPos.x / SNAP_STEP) * SNAP_STEP;
                    localPos.y = Mathf.Round(localPos.y / SNAP_STEP) * SNAP_STEP;
                }

                spline.SetPosition(i, localPos);
                controller.RefreshSpriteShape();
            }
        }
    }
}
