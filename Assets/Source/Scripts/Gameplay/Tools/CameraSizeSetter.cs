using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class CameraSizeSetter : MonoBehaviour
    {
        [SerializeField] private float _horizontalOffset;
        private ILevelProvider _levelProvider;
        private Camera _camera;

        [Inject]
        public void Construct(ILevelProvider levelProvider)
        {
            _levelProvider = levelProvider;
        }

        private void Start()
        {
            _camera = Camera.main;
            SetCameraSize();
            Debug.Log("Set Camera size");
        }

        private void SetCameraSize()
        {
            Bounds bounds = GetMaxBounds(_levelProvider.RotablePart);
            float maxWidth = bounds.size.magnitude + _horizontalOffset *2;

            float aspect = _camera.aspect;

            _camera.orthographicSize = maxWidth / (2f * aspect);
        }

        private Bounds GetMaxBounds(Transform parent)
        {
            Renderer[] renderers = parent.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
                return new Bounds(parent.position, Vector3.zero);

            Bounds bounds = renderers[0].bounds;

            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            return bounds;
        }
    }
}
