using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class CameraSizeSetterByVertices : MonoBehaviour
    {
        [SerializeField] private float _horizontalOffset;

        private ILevelProvider _levelProvider;
        private Camera _camera;

        private readonly List<Vector2> _points = new();
        private readonly List<Vector2> _hull = new();

        [Inject]
        public void Construct(ILevelProvider levelProvider)
        {
            _levelProvider = levelProvider;
        }

        private void Start()
        {
            _camera = Camera.main;
            SetCameraSize();
        }

        private void SetCameraSize()
        {
            float maxDistance = GetMaxDistance(_levelProvider.RotablePart);
            float maxWidth = maxDistance + _horizontalOffset * 2f;

            _camera.orthographicSize = maxWidth / (2f * _camera.aspect);
        }

        private float GetMaxDistance(Transform parent)
        {
            _points.Clear();
            _hull.Clear();

            var shapes = parent.GetComponentsInChildren<SpriteShapeController>();

            foreach (var shape in shapes)
            {
                var spline = shape.spline;
                var tr = shape.transform;

                int count = spline.GetPointCount();
                for (int i = 0; i < count; i++)
                {
                    Vector3 world = tr.TransformPoint(spline.GetPosition(i));
                    _points.Add(new Vector2(world.x, world.y));
                }
            }
            if (_points.Count < 2)
                return 0f;

            BuildConvexHull(_points, _hull);

            return RotatingCalipersDiameter(_hull);
        }

        #region Convex Hull (Monotone Chain)

        private void BuildConvexHull(List<Vector2> points, List<Vector2> hull)
        {
            points.Sort((a, b) =>
                a.x == b.x ? a.y.CompareTo(b.y) : a.x.CompareTo(b.x));

            for (int i = 0; i < points.Count; i++)
            {
                while (hull.Count >= 2 &&
                       Cross(hull[^2], hull[^1], points[i]) <= 0)
                    hull.RemoveAt(hull.Count - 1);

                hull.Add(points[i]);
            }

            int lowerCount = hull.Count;

            for (int i = points.Count - 2; i >= 0; i--)
            {
                while (hull.Count > lowerCount &&
                       Cross(hull[^2], hull[^1], points[i]) <= 0)
                    hull.RemoveAt(hull.Count - 1);

                hull.Add(points[i]);
            }

            hull.RemoveAt(hull.Count - 1);
        }

        private float Cross(Vector2 o, Vector2 a, Vector2 b)
        {
            return (a.x - o.x) * (b.y - o.y) -
                   (a.y - o.y) * (b.x - o.x);
        }

        #endregion

        #region Rotating Calipers (Diameter)
        float RotatingCalipersDiameter(List<Vector2> hull)
        {
            int n = hull.Count;
            float maxSqr = 0f;

            for (int i = 0; i < n; i++)
                for (int j = i + 1; j < n; j++)
                    maxSqr = Mathf.Max(maxSqr, (hull[i] - hull[j]).sqrMagnitude);

            return Mathf.Sqrt(maxSqr);
        }

        #endregion
    }
}