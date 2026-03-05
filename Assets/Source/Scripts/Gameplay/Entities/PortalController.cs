using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class PortalController : MonoBehaviour
    {
        [SerializeField] private Portal _portalA;
        [SerializeField] private Portal _portalB;

        private Dictionary<Rigidbody2D, GameObject> _clones =
            new Dictionary<Rigidbody2D, GameObject>();

        private HashSet<Rigidbody2D> _cooldown =
            new HashSet<Rigidbody2D>();

        private void Awake()
        {
            _portalA.Link(_portalB);
            _portalB.Link(_portalA);

            _portalA.Initialize(this);
            _portalB.Initialize(this);
        }

        public void RegisterObject(Portal portal, Rigidbody2D rb)
        {
            if (_clones.ContainsKey(rb) || _cooldown.Contains(rb))
                return;

            GameObject clone = Instantiate(
                rb.gameObject,
                rb.transform.position,
                rb.transform.rotation);

            Destroy(clone.GetComponent<Rigidbody2D>());
            _clones.Add(rb, clone);
        }

        public void UpdateObject(Portal entryPortal, Rigidbody2D rb)
        {
            if (!_clones.TryGetValue(rb, out GameObject clone))
                return;

            Portal exitPortal = entryPortal.LinkedPortal;

            // Обновляем клон
            clone.transform.position =
                entryPortal.TransformPositionToLinked(rb.position);

            float deltaRot =
                exitPortal.transform.eulerAngles.z -
                entryPortal.transform.eulerAngles.z;

            clone.transform.rotation =
                Quaternion.Euler(0, 0,
                rb.rotation + deltaRot);

            // Проверяем пересечение плоскости
            Vector2 portalNormal = entryPortal.transform.up;
            Vector2 toObject = rb.position -
                               (Vector2)entryPortal.transform.position;

            if (Vector2.Dot(toObject, portalNormal) > 0f)
            {
                Teleport(entryPortal, rb);
            }
        }

        private void Teleport(Portal entryPortal, Rigidbody2D rb)
        {
            Portal exitPortal = entryPortal.LinkedPortal;

            rb.position =
                entryPortal.TransformPositionToLinked(rb.position);

            rb.velocity =
                entryPortal.TransformDirectionToLinked(rb.velocity);

            float deltaRot =
                exitPortal.transform.eulerAngles.z -
                entryPortal.transform.eulerAngles.z;

            rb.rotation += deltaRot;

            CleanupClone(rb);

            _cooldown.Add(rb);
            StartCoroutine(RemoveCooldownNextFrame(rb));
        }

        private System.Collections.IEnumerator RemoveCooldownNextFrame(Rigidbody2D rb)
        {
            yield return null;
            _cooldown.Remove(rb);
        }

        public void UnregisterObject(Rigidbody2D rb)
        {
            CleanupClone(rb);
        }

        private void CleanupClone(Rigidbody2D rb)
        {
            if (_clones.TryGetValue(rb, out GameObject clone))
            {
                Destroy(clone);
                _clones.Remove(rb);
            }
        }
    }
}