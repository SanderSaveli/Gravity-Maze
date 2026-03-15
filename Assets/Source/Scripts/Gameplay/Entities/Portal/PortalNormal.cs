using UnityEngine;

[ExecuteAlways]
public class PortalNormal : MonoBehaviour
{
    [Header("Portal dimensions")]
    [Tooltip("Ширина портала по тангенциальной оси (перпендикулярно нормали)")]
    public float width = 2f;

    [Tooltip("Глубина портала вдоль нормали")]
    public float depth = 0.5f;

    [Header("Portal direction")]
    [Tooltip("Направление нормали, куда игрок будет телепортироваться (1 или -1)")]
    public int normalDirection = 1;

    [Header("Plane offset")]
    [Tooltip("Смещение плоскости портала относительно Transform.position в локальных координатах")]
    public Vector2 planeOffset = Vector2.zero;

    /// <summary>
    /// Нормализованная нормаль портала с учётом поворота объекта и направления
    /// </summary>
    public Vector2 PortalNormalVector => (Vector2)transform.right * normalDirection;

    /// <summary>
    /// Центр плоскости портала с учётом смещения в локальной системе
    /// </summary>
    public Vector2 PlanePosition
    {
        get
        {
            // planeOffset — локальные координаты относительно трансформа
            Vector3 offsetWorld = transform.TransformVector(new Vector3(planeOffset.x, planeOffset.y, 0f));
            return (Vector2)transform.position + (Vector2)offsetWorld;
        }
    }

    /// <summary>
    /// Получить линию пересечения портала (для телепорта)
    /// Возвращает два конца линии в мировых координатах
    /// </summary>
    public void GetPortalLine(out Vector2 start, out Vector2 end)
    {
        Vector2 tangent = transform.up.normalized;
        Vector2 center = PlanePosition;

        start = center - tangent * width * 0.5f;
        end = center + tangent * width * 0.5f;
    }

    private void OnDrawGizmos()
    {
        Vector2 normal = PortalNormalVector.normalized;
        Vector2 tangent = transform.up.normalized;
        Vector2 center = PlanePosition;

        // Прямоугольник портала (синий)
        Vector2 halfTangent = tangent * (width * 0.5f);
        Vector2 halfNormal = normal * (depth * 0.5f);

        Vector3 bl = center - halfTangent - halfNormal;
        Vector3 br = center + halfTangent - halfNormal;
        Vector3 tr = center + halfTangent + halfNormal;
        Vector3 tl = center - halfTangent + halfNormal;

        Gizmos.color = new Color(0f, 0.5f, 1f, 0.2f);
        Gizmos.DrawLine(bl, br);
        Gizmos.DrawLine(br, tr);
        Gizmos.DrawLine(tr, tl);
        Gizmos.DrawLine(tl, bl);

        // Стрелка нормали (синяя)
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(center, normal * 1f);
    }
}