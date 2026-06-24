using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class RotatableScreenBorderCollider : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _borderWidth = 0.5f;

        private EdgeCollider2D _edgeCollider;

        private Vector3 _lastCamPos;
        private Quaternion _lastCamRot;
        private float _lastCamSize;
        private float _lastAspect;

        private void Awake()
        {
            _edgeCollider = GetComponent<EdgeCollider2D>();

            if (_camera == null)
                _camera = Camera.main;

            RebuildBorder();
            CacheCameraState();
        }

        private void LateUpdate()
        {
            if (CameraChanged())
            {
                RebuildBorder();
                CacheCameraState();
            }
        }

        private bool CameraChanged()
        {
            return _camera.transform.position != _lastCamPos
                   || _camera.transform.rotation != _lastCamRot
                   || _camera.orthographicSize != _lastCamSize
                   || _camera.aspect != _lastAspect;
        }

        private void CacheCameraState()
        {
            _lastCamPos = _camera.transform.position;
            _lastCamRot = _camera.transform.rotation;
            _lastCamSize = _camera.orthographicSize;
            _lastAspect = _camera.aspect;
        }

        private void RebuildBorder()
        {
            Vector3 bl3 = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            Vector3 tl3 = _camera.ViewportToWorldPoint(new Vector3(0, 1, _camera.nearClipPlane));
            Vector3 tr3 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
            Vector3 br3 = _camera.ViewportToWorldPoint(new Vector3(1, 0, _camera.nearClipPlane));

            Vector2 center = (bl3 + tr3) * 0.5f;

            Vector2 bottomLeft = ExpandFromCenter(bl3, center);
            Vector2 topLeft = ExpandFromCenter(tl3, center);
            Vector2 topRight = ExpandFromCenter(tr3, center);
            Vector2 bottomRight = ExpandFromCenter(br3, center);

            _edgeCollider.SetPoints(new List<Vector2>
            {
                bottomLeft,
                topLeft,
                topRight,
                bottomRight,
                bottomLeft
            });

            _edgeCollider.edgeRadius = _borderWidth;
        }

        private Vector2 ExpandFromCenter(Vector2 point, Vector2 center)
        {
            Vector2 dir = (point - center).normalized;
            return point + dir * _borderWidth;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_edgeCollider == null)
                _edgeCollider = GetComponent<EdgeCollider2D>();

            if (_camera == null)
                _camera = Camera.main;

            if (_camera != null)
                RebuildBorder();
        }
#endif
    }
}