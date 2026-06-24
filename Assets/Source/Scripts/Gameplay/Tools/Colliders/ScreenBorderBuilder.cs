using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class ScreenBorderBuilder : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _thickness = 0.5f;

        [SerializeField] private Transform _top;
        [SerializeField] private Transform _bottom;
        [SerializeField] private Transform _left;
        [SerializeField] private Transform _right;

        private void Start()
        {
            if (_camera == null)
                _camera = Camera.main;

            Build();
        }

        private void Build()
        {
            float height = _camera.orthographicSize * 2f;
            float width = height * _camera.aspect;

            _top.localPosition = new Vector3(0, height / 2f, 0);
            _bottom.localPosition = new Vector3(0, -height / 2f, 0);
            _left.localPosition = new Vector3(-width / 2f, 0, 0);
            _right.localPosition = new Vector3(width / 2f, 0, 0);

            _top.localScale = new Vector3(width, _thickness, 1);
            _bottom.localScale = new Vector3(width, _thickness, 1);
            _left.localScale = new Vector3(_thickness, height, 1);
            _right.localScale = new Vector3(_thickness, height, 1);
        }
    }
}