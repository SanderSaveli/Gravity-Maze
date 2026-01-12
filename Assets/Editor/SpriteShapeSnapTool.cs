using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.U2D;

[EditorTool("SpriteShape Snap Tool")]
public class SpriteShapeSnapTool : EditorTool
{
    private const float SNAP_STEP = 1f;
    private const int GRID_EXTENT = 50; // размер сетки в шагах

    public override void OnToolGUI(EditorWindow window)
    {
        var controller = Selection.activeGameObject?.GetComponent<SpriteShapeController>();
        if (controller == null)
            return;

        DrawWorldGrid();

        var spline = controller.spline;
        Event e = Event.current;

        for (int i = 0; i < spline.GetPointCount(); i++)
        {
            Vector3 worldPos =
                controller.transform.TransformPoint(spline.GetPosition(i));

            EditorGUI.BeginChangeCheck();
            Vector3 newWorldPos =
                Handles.PositionHandle(worldPos, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(controller, "Move SpriteShape Point");

                // 🔹 SNAP ПО УМОЛЧАНИЮ, Shift — свободно
                if (!e.shift)
                {
                    newWorldPos = SnapWorldPosition(newWorldPos);
                }

                Vector3 localPos =
                    controller.transform.InverseTransformPoint(newWorldPos);

                spline.SetPosition(i, localPos);
                controller.RefreshSpriteShape();
            }
        }
    }

    // =============================
    // SNAP
    // =============================
    private Vector3 SnapWorldPosition(Vector3 pos)
    {
        pos.x = Mathf.Round(pos.x / SNAP_STEP) * SNAP_STEP;
        pos.y = Mathf.Round(pos.y / SNAP_STEP) * SNAP_STEP;
        return pos;
    }

    // =============================
    // GRID
    // =============================
    private void DrawWorldGrid()
    {
        Handles.color = new Color(1f, 1f, 1f, 0.15f);

        for (int x = -GRID_EXTENT; x <= GRID_EXTENT; x++)
        {
            float wx = x * SNAP_STEP;
            Handles.DrawLine(
                new Vector3(wx, -GRID_EXTENT * SNAP_STEP, 0),
                new Vector3(wx, GRID_EXTENT * SNAP_STEP, 0)
            );
        }

        for (int y = -GRID_EXTENT; y <= GRID_EXTENT; y++)
        {
            float wy = y * SNAP_STEP;
            Handles.DrawLine(
                new Vector3(-GRID_EXTENT * SNAP_STEP, wy, 0),
                new Vector3(GRID_EXTENT * SNAP_STEP, wy, 0)
            );
        }
    }
}
