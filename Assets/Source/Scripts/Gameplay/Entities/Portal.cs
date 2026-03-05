using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(Collider2D))]
    public class Portal : MonoBehaviour
    {
        public Portal LinkedPortal { get; private set; }

        private PortalController _controller;

        public void Initialize(PortalController controller)
        {
            _controller = controller;
        }

        public void Link(Portal other)
        {
            LinkedPortal = other;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (LinkedPortal == null) return;

            if (other.attachedRigidbody != null)
            {
                _controller.RegisterObject(this, other.attachedRigidbody);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (LinkedPortal == null) return;

            if (other.attachedRigidbody != null)
            {
                _controller.UpdateObject(this, other.attachedRigidbody);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (LinkedPortal == null) return;

            if (other.attachedRigidbody != null)
            {
                _controller.UnregisterObject(other.attachedRigidbody);
            }
        }

        public Vector2 TransformPositionToLinked(Vector2 worldPos)
        {
            Vector2 localPos = transform.InverseTransformPoint(worldPos);
            return LinkedPortal.transform.TransformPoint(localPos);
        }

        public Vector2 TransformDirectionToLinked(Vector2 worldDir)
        {
            Vector2 localDir = transform.InverseTransformDirection(worldDir);
            return LinkedPortal.transform.TransformDirection(localDir);
        }
    }
}