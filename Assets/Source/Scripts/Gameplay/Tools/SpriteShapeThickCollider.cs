using UnityEngine;
using UnityEngine.U2D;

[ExecuteAlways] // Позволяет работать в редакторе
[RequireComponent(typeof(SpriteShapeController))]
[RequireComponent(typeof(PolygonCollider2D))]
public class SpriteShapeThickCollider : MonoBehaviour
{
    [Tooltip("Толщина линии платформы в юнитах")]
    public float lineThickness = 1f;

    private SpriteShapeController ssc;
    private PolygonCollider2D polyCollider;

    private Vector3[] lastPositions;

    void OnEnable()
    {
        ssc = GetComponent<SpriteShapeController>();
        polyCollider = GetComponent<PolygonCollider2D>();
        UpdateCollider();
    }

    void Update()
    {
        if (ssc == null) return;

        // Проверяем, изменились ли точки сплайна
        bool changed = false;
        int count = ssc.spline.GetPointCount();

        if (lastPositions == null || lastPositions.Length != count)
        {
            changed = true;
            lastPositions = new Vector3[count];
        }

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = ssc.spline.GetPosition(i);
            if (lastPositions[i] != pos)
            {
                changed = true;
                lastPositions[i] = pos;
            }
        }

        // Проверяем изменение толщины
        if (!changed && polyCollider != null && polyCollider.pathCount > 0)
        {
            // Добавим небольшую проверку для изменения толщины
            if (Mathf.Abs(lineThickness - polyCollider.bounds.size.y) > 0.001f)
            {
                changed = true;
            }
        }

        if (changed)
        {
            UpdateCollider();
        }
    }

    [ContextMenu("Update Collider")]
    public void UpdateCollider()
    {
        var spline = ssc.spline;
        int pointsCount = spline.GetPointCount();
        if (pointsCount < 2)
        {
            Debug.LogWarning("Сплайн слишком короткий для генерации коллайдера.");
            return;
        }

        Vector2[] topEdge = new Vector2[pointsCount];
        Vector2[] bottomEdge = new Vector2[pointsCount];

        for (int i = 0; i < pointsCount; i++)
        {
            Vector3 point = spline.GetPosition(i);

            // Направление сегмента
            Vector3 dir;
            if (i < pointsCount - 1)
                dir = (spline.GetPosition(i + 1) - point).normalized;
            else
                dir = (point - spline.GetPosition(i - 1)).normalized;

            // Нормаль для смещения по толщине
            Vector3 normal = new Vector3(-dir.y, dir.x, 0);

            topEdge[i] = (Vector2)(point + normal * lineThickness * 0.5f);
            bottomEdge[i] = (Vector2)(point - normal * lineThickness * 0.5f);
        }

        // Замкнутый массив вершин
        Vector2[] vertices = new Vector2[pointsCount * 2];
        for (int i = 0; i < pointsCount; i++)
        {
            vertices[i] = topEdge[i];
            vertices[vertices.Length - 1 - i] = bottomEdge[i];
        }

        polyCollider.pathCount = 1;
        polyCollider.SetPath(0, vertices);
    }
}
