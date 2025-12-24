using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class ScreenBorrderCollider : MonoBehaviour
    {
        [SerializeField] private float _borderWidth;
        [SerializeField] Camera _camera;

        private EdgeCollider2D _edgeCollider;

        private void Reset()
        {
            _edgeCollider = GetComponent<EdgeCollider2D>();
        }

        private async void Start()
        {
            await UniTask.Yield();
            await UniTask.Yield();
            BuildBorederCollider();
            Debug.Log("Build Birder size");
        }

        private void OnValidate()
        {
            if (_camera != null)
            {
                BuildBorederCollider();
            }
        }

        private void BuildBorederCollider()
        {
            if(_edgeCollider == null)
            {
                _edgeCollider = GetComponent<EdgeCollider2D>();
            }
            Vector2 min = _camera.ViewportToWorldPoint(Vector2.zero);
            Vector2 max = _camera.ViewportToWorldPoint(Vector2.one);
            Vector2 widthOffset = new Vector2(_borderWidth, _borderWidth);
            min -= widthOffset;
            max += widthOffset;

            Vector2 bottomLeft = new Vector2(min.x, min.y);
            Vector2 upperLeft = new Vector2(min.x, max.y);
            Vector2 upperRight = new Vector2(max.x, max.y);
            Vector2 bottomRight = new Vector2(max.x, min.y);

            _edgeCollider.SetPoints(new List<Vector2> { bottomLeft, upperLeft, upperRight, bottomRight, bottomLeft });
            _edgeCollider.edgeRadius = _borderWidth;
        }
    }
}
