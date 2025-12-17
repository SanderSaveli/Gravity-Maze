using UnityEngine;
using UnityEngine.U2D;

[ExecuteAlways]
public class SpriteShapeEndObjects : MonoBehaviour
{
    [SerializeField] private SpriteShapeController spriteShape;
    [SerializeField] private Transform startObject;
    [SerializeField] private Transform endObject;
    [SerializeField] private float scaleFactor = 1f;

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying)
            UpdateObjects();
    }
#endif

    private void OnEnable()
    {
        UpdateObjects();
    }

    private void UpdateObjects()
    {
        if (spriteShape == null)
            return;

        var spline = spriteShape.spline;
        int count = spline.GetPointCount();
        if (count < 2)
            return;

        UpdateStartObject(spline, 1);
        UpdateEndObject(spline, count - 1, count -2);
    }

    private void UpdateStartObject(Spline spline, int neighborIndex)
    {
        if (startObject == null)
            return;

        int index = 0;

        Vector3 localPos = spline.GetPosition(index);
        Vector3 localTangent = spline.GetRightTangent(index);

        ApplyTransform(startObject, localPos, localTangent, index, neighborIndex, spline);
    }

    private void UpdateEndObject(Spline spline, int index, int neighborIndex)
    {
        if (endObject == null)
            return;

        Vector3 localPos = spline.GetPosition(index);
        Vector3 localTangent = spline.GetLeftTangent(index);

        ApplyTransform(endObject, localPos, localTangent, index, neighborIndex, spline);
    }

    private void ApplyTransform(
        Transform target,
        Vector3 localPosition,
        Vector3 localTangent,
        int pointIndex,
        int neighborIndex,
        Spline spline)
    {
        // Позиция
        target.position = spriteShape.transform.TransformPoint(localPosition);

        Vector3 worldDir;

        // === 1. Пытаемся использовать тангент ===
        if (localTangent.sqrMagnitude > 0.0001f)
        {
            worldDir = spriteShape.transform.TransformVector(localTangent);
        }
        // === 2. Fallback для Linear / Corner ===
        else
        {
            Vector3 p0 = spline.GetPosition(pointIndex);
            Vector3 p1 = spline.GetPosition(neighborIndex);
            worldDir = spriteShape.transform.TransformVector(p0 - p1);

            // Для старта инвертируем
            if (pointIndex == 0)
                worldDir = -worldDir;
        }

        // Вращение
        if (worldDir.sqrMagnitude > 0.0001f)
        {
            worldDir.Normalize();
            float angle = Mathf.Atan2(worldDir.y, worldDir.x) * Mathf.Rad2Deg;
            target.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        // Масштаб
        float scale = spline.GetHeight(pointIndex) * scaleFactor;
        target.localScale = Vector3.one * scale;
    }
}
