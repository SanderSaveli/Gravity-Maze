using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ScreenSideCollider : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [SerializeField] private float _height = 1f;
        [SerializeField] private float _heightOffset = 0f;

        [Header("Viewport Width Offsets")]
        [SerializeField] private float _widthLeftOffset;

        [SerializeField] private float _widthRightOffset;
        [SerializeField] private bool _editGameObjectScale;
        [SerializeField] private Direction _direction;
        private BoxCollider2D _collider;


        private void Reset()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        private async void Start()
        {
            await UniTask.Yield();
            _collider = GetComponent<BoxCollider2D>();
            BuildBorederCollider();
            Debug.Log("Build Side Collider size");
        }

        private void OnValidate()
        {
            if (_camera == null)
                return;

            if (_collider == null)
                _collider = GetComponent<BoxCollider2D>();

            BuildBorederCollider();
        }

        private void BuildBorederCollider()
        {
            float left = Mathf.Clamp01(_widthLeftOffset);
            float right = Mathf.Clamp01(1f - _widthRightOffset);

            Vector2 center = Vector2.zero;
            Vector2 worldSize = Vector2.zero;

            switch (_direction)
            {
                case Direction.Up:
                    {
                        Vector2 bl = _camera.ViewportToWorldPoint(new Vector2(left, 1f));
                        Vector2 br = _camera.ViewportToWorldPoint(new Vector2(right, 1f));

                        float width = br.x - bl.x;

                        center = new Vector2(
                            (bl.x + br.x) * 0.5f,
                            bl.y - _height * 0.5f + _heightOffset
                        );

                        worldSize = new Vector2(width, _height);
                        break;
                    }

                case Direction.Down:
                    {
                        Vector2 tl = _camera.ViewportToWorldPoint(new Vector2(left, 0f));
                        Vector2 tr = _camera.ViewportToWorldPoint(new Vector2(right, 0f));

                        float width = tr.x - tl.x;

                        center = new Vector2(
                            (tl.x + tr.x) * 0.5f,
                            tl.y + _height * 0.5f - _heightOffset
                        );

                        worldSize = new Vector2(width, _height);
                        break;
                    }

                case Direction.Left:
                    {
                        Vector2 bl = _camera.ViewportToWorldPoint(new Vector2(0f, left));
                        Vector2 tl = _camera.ViewportToWorldPoint(new Vector2(0f, right));

                        float height = tl.y - bl.y;

                        center = new Vector2(
                            bl.x + _height * 0.5f - _heightOffset,
                            (bl.y + tl.y) * 0.5f
                        );

                        worldSize = new Vector2(_height, height);
                        break;
                    }

                case Direction.Right:
                    {
                        Vector2 br = _camera.ViewportToWorldPoint(new Vector2(1f, left));
                        Vector2 tr = _camera.ViewportToWorldPoint(new Vector2(1f, right));

                        float height = tr.y - br.y;

                        center = new Vector2(
                            br.x - _height * 0.5f + _heightOffset,
                            (br.y + tr.y) * 0.5f
                        );

                        worldSize = new Vector2(_height, height);
                        break;
                    }
            }

            ApplyCollider(center, worldSize);
        }

        private void ApplyCollider(Vector2 worldCenter, Vector2 worldSize)
        {
            transform.position = new Vector3(worldCenter.x, worldCenter.y, transform.position.z);

            _collider.offset = Vector2.zero;

            if (_editGameObjectScale)
            {
                //  оллайдер фиксированный
                _collider.size = Vector2.one;

                transform.localScale = new Vector3(
                    worldSize.x,
                    worldSize.y,
                    1f
                );
            }
            else
            {
                // Scale не трогаем
                transform.localScale = Vector3.one;
                _collider.size = worldSize;
            }
        }
    }
}
