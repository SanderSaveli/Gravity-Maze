using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(TrailRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class TrailController : MonoBehaviour
    {
        public float speedThreshold = 5f;
        public float extraPointSpacing = 0.1f;

        [Header("Teleport Detection")]
        public float teleportDistanceThreshold = 2f;

        private TrailRenderer trail;
        private Rigidbody2D rb;

        private Vector3 lastPosition;
        private bool isTeleporting = false;

        void Awake()
        {
            trail = GetComponent<TrailRenderer>();
            rb = GetComponent<Rigidbody2D>();

            lastPosition = transform.position;
            trail.emitting = true;
        }

        void LateUpdate()
        {
            float distance = Vector3.Distance(transform.position, lastPosition);

            // 🔥 Детект телепорта
            if (!isTeleporting && distance > teleportDistanceThreshold)
            {
                TeleportFix().Forget();
            }

            float speed = rb.velocity.magnitude;

            if (speed > speedThreshold && !isTeleporting)
            {
                AddInterpolatedPoints(lastPosition, transform.position);
            }

            lastPosition = transform.position;
        }

        void AddInterpolatedPoints(Vector3 from, Vector3 to)
        {
            float distance = Vector3.Distance(from, to);
            if (distance <= 0.0001f) return;

            int steps = Mathf.CeilToInt(distance / Mathf.Max(extraPointSpacing, 0.001f));

            for (int i = 1; i < steps; i++)
            {
                float t = (float)i / steps;
                Vector3 pos = Vector3.Lerp(from, to, t);
                trail.AddPosition(pos);
            }
        }

        async UniTask TeleportFix()
        {
            isTeleporting = true;
            trail.enabled = false;
            trail.emitting = false;
            trail.Clear();

            await UniTask.DelayFrame(1);
            if(gameObject != null)
            {
                lastPosition = transform.position;
                trail.emitting = true;

                isTeleporting = false;
                trail.enabled = true;
            }
        }
    }
}