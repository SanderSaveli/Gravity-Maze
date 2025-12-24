using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace SanderSaveli.GravityMaze
{
    public class PolygonShapeCreator : ShapeCreator
    {
        private int _sides = 3;
        private float _rotation = 0f;
        private float _radius = 1f;
        private bool _isCreateHole = false;
        private const float _initialRotation = 90;
        private float _holeSegment = 0;
        private float _holeRotation;

        public override void Draw()
        {
            _sides = EditorGUILayout.IntSlider("Number of Sides", _sides, 3, 12);
            _rotation = EditorGUILayout.Slider("Rotation (degrees)", _rotation, 0f, 360f);
            _radius = EditorGUILayout.FloatField("Radius", _radius);

            _isCreateHole = GUILayout.Toggle(_isCreateHole, "IsCreateHole");
            if (_isCreateHole)
            {
                _holeSegment = EditorGUILayout.Slider("Hole segment (degrees)", _holeSegment, 0f, 360f);
                _holeRotation = EditorGUILayout.Slider("Hole rotation (degrees)", _holeRotation, 0f, 360f);
            }
        }

        public override void Create(SpriteShapeController controller)
        {
            if (controller == null || controller.spline == null)
                return;

            Spline spline = controller.spline;
            Undo.RecordObject(controller, "Convert to Polygon");

            List<ShapeVertex> vertices = GetVertises();
            spline.isOpenEnded = false;

            if (_isCreateHole && _holeSegment > 0)
            {
                vertices = CreateHole(vertices);
                spline.isOpenEnded = true;
            }

            ApplyToSpline(vertices, spline);

            if (controller.gameObject.TryGetComponent(out SpriteShapeEndObjects component))
            {
                component.IsActivate = spline.isOpenEnded;
            }
        }

        private void TryAddPoint(List<ShapeVertex> list, ShapeVertex point)
        {
            float minDist = 0.05f;
            foreach (ShapeVertex v in list)
            {
                if (Vector3.Distance(v.Pos, point.Pos) < minDist)
                    return;
            }

            list.Add(point);
        }

        private void ApplyToSpline(List<ShapeVertex> points, Spline spline)
        {
            MoveSplinePointsToSafeArea(spline);
            SetPoints(points, spline);
        }

        private float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle < 0f) angle += 360f;
            return angle;
        }

        private bool IsAngleInHole(float angle, float start, float end)
        {
            if (start <= end)
                return angle >= start && angle <= end;
            else
                return angle >= start || angle <= end;
        }

        private void SetPoints(List<ShapeVertex> positions, Spline spline)
        {
            int currentPoints = spline.GetPointCount();

            for (int i = 0; i < positions.Count; i++)
            {
                if (i < currentPoints)
                {
                    spline.SetPosition(i, positions[i].Pos);
                }
                else
                {
                    spline.InsertPointAt(i, positions[i].Pos);
                }
                spline.SetTangentMode(i, ShapeTangentMode.Linear);
            }
            while (positions.Count < currentPoints)
            {
                spline.RemovePointAt(currentPoints - 1);
                currentPoints = spline.GetPointCount();
            }
        }

        private void MoveSplinePointsToSafeArea(Spline spline)
        {
            int count = spline.GetPointCount();
            Vector3 offset = Vector3.forward * 20;

            for (int i = 0; i < count; i++)
            {
                Vector3 p = spline.GetPosition(i);
                spline.SetPosition(i, p + offset);
            }
        }

        private List<ShapeVertex> GetVertises()
        {
            float angleStep = 360f / _sides;
            float baseRotation = _initialRotation + _rotation;
            List<ShapeVertex> vertices = new();

            for (int i = 0; i < _sides; i++)
            {
                float angleDeg = baseRotation + i * angleStep;
                float angleRad = angleDeg * Mathf.Deg2Rad;

                Vector3 p = new Vector3(
                    Mathf.Cos(angleRad) * _radius,
                    Mathf.Sin(angleRad) * _radius,
                    0f
                );
                ShapeVertex vertex = new ShapeVertex(p, NormalizeAngle(angleDeg));
                vertices.Add(vertex);
            }
            return vertices;
        }

        private List<ShapeVertex> CreateHole(List<ShapeVertex> shapeVertices)
        {
            float holeCenter = NormalizeAngle(
                _initialRotation +
                _rotation +
                _holeRotation
            );

            float half = _holeSegment * 0.5f;
            float holeStart = NormalizeAngle(holeCenter - half);
            float holeEnd = NormalizeAngle(holeCenter + half);

            List<ShapeVertex> result = new();

            int count = shapeVertices.Count;
            for (int i = 0; i < count; i++)
            {
                ShapeVertex current = shapeVertices[i];
                ShapeVertex next = shapeVertices[(i + 1) % count];

                bool currInside = IsAngleInHole(current.Angle, holeStart, holeEnd);
                bool nextInside = IsAngleInHole(next.Angle, holeStart, holeEnd);

                if (!currInside)
                    TryAddPoint(result, current);

                if (currInside != nextInside)
                {
                    float borderAngle = currInside ? holeEnd : holeStart;
                    if (TryGetEdgeIntersection(current, next, borderAngle, out Vector3 intersection))
                    {
                        TryAddPoint(result, new ShapeVertex(intersection, borderAngle));
                    }
                }
            }
            shapeVertices = result;
            shapeVertices = ReorderToHole(shapeVertices, holeEnd);
            return shapeVertices;
        }

        private List<ShapeVertex> ReorderToHole(List<ShapeVertex> shapeVertices, float holeEnd)
        {
            if (!_isCreateHole || _holeSegment <= 0f || shapeVertices.Count < 2)
            {
                return shapeVertices;
            }
            int rightIndex = 0;
            float minDistRight = float.MaxValue;

            for (int i = 0; i < shapeVertices.Count; i++)
            {
                float angle = Mathf.Atan2(shapeVertices[i].Pos.y, shapeVertices[i].Pos.x) * Mathf.Rad2Deg;
                angle = NormalizeAngle(angle);
                float dist = Mathf.Abs(Mathf.DeltaAngle(angle, holeEnd));
                if (dist < minDistRight)
                {
                    minDistRight = dist;
                    rightIndex = i;
                }
            }

            List<ShapeVertex> reordered = new List<ShapeVertex>();
            for (int i = 0; i < shapeVertices.Count; i++)
            {
                int idx = (rightIndex + i) % shapeVertices.Count;
                reordered.Add(shapeVertices[idx]);
            }

            return reordered;
        }

        private bool TryGetEdgeIntersection(
            ShapeVertex a,
            ShapeVertex b,
            float angleDeg,
            out Vector3 intersection)
        {
            Vector2 p1 = a.Pos;
            Vector2 p2 = b.Pos;

            Vector2 dir = new Vector2(
                Mathf.Cos(angleDeg * Mathf.Deg2Rad),
                Mathf.Sin(angleDeg * Mathf.Deg2Rad)
            );
            Vector2 edge = p2 - p1;

            float cross = dir.x * edge.y - dir.y * edge.x;
            if (Mathf.Abs(cross) < 0.00001f)
            {
                intersection = Vector3.zero;
                return false;
            }

            Vector2 diff = p1;

            float t = (diff.x * edge.y - diff.y * edge.x) / cross;
            float u = (diff.x * dir.y - diff.y * dir.x) / cross;

            if (t < 0f || u < 0f || u > 1f)
            {
                intersection = Vector3.zero;
                return false;
            }

            intersection = (Vector3)(dir * t);
            return true;
        }
    }
}
