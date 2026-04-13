using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class SnapScroll : MonoBehaviour, IEndDragHandler, IBeginDragHandler
    {
        [Header("Components")]
        [SerializeField] protected ScrollRect _scrollRect;
        [SerializeField] protected RectTransform _viewport;
        [SerializeField] protected RectTransform _content;

        [Header("Settings")]
        [SerializeField] protected Vector2 _offset;
        [SerializeField] protected float _snapDuration = 0.25f;
        [SerializeField] protected Ease _ease = Ease.Linear;
        [SerializeField] protected float _velocityThreshold = 150f;
        [SerializeField] protected bool _isDebugEnable = false;

        private Coroutine _snapRoutine;
        private readonly Vector3[] _corners = new Vector3[4];

        private void OnEnable()
        {
            _snapRoutine = StartCoroutine(SnapWhenStopped());
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_snapRoutine != null)
                StopCoroutine(_snapRoutine);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_snapRoutine != null)
                StopCoroutine(_snapRoutine);

            _snapRoutine = StartCoroutine(SnapWhenStopped());
        }

        private IEnumerator SnapWhenStopped()
        {
            yield return null;
            yield return new WaitUntil(() => _scrollRect.velocity.magnitude <= _velocityThreshold);
            _scrollRect.velocity = Vector3.zero;
            if (_content.childCount == 0)
            {
                _snapRoutine = null;
                yield break;
            }

            Vector3 viewportCenterWorld = GetViewportCenter();

            RectTransform nearest = _content.Cast<RectTransform>()
                .OrderBy(child => Vector3.SqrMagnitude(GetWorldCenter(child) - viewportCenterWorld))
                .First();

            SnapTo(nearest, viewportCenterWorld);

            _snapRoutine = null;
        }

        public void SnapTo(RectTransform target)
        {
            Vector3 viewportCenterWorld = GetViewportCenter();
            SnapTo(target, viewportCenterWorld);
        }

        public void SnapImmediatley(RectTransform target)
        {
            StopCoroutine(_snapRoutine);
            _scrollRect.StopMovement();
            Vector3 viewportCenterWorld = GetViewportCenter();
            Vector2 newAnchoredPos = GetTargetAnchoredPosition(target, viewportCenterWorld);
            _content.anchoredPosition = newAnchoredPos;
        }

        protected virtual void SnapTo(RectTransform target, Vector3 viewportCenterWorld)
        {
            _scrollRect.StopMovement();
            DOTween.Kill(_content);

            Vector2 newAnchoredPos = GetTargetAnchoredPosition(target, viewportCenterWorld);
            MoveTo(_content, newAnchoredPos);
        }

        protected void MoveTo(RectTransform target, Vector2 newAnchoredPos)
        {
            DOTween.To(
                () => target.anchoredPosition,
                v => target.anchoredPosition = v,
                newAnchoredPos,
                _snapDuration
            )
            .SetEase(_ease)
            .SetLink(target.gameObject);
        }

        protected Vector3 GetViewportCenter()
        {
            Vector3 viewportCenterWorld = GetWorldCenter(_viewport);
            viewportCenterWorld += (Vector3)_offset;
            return viewportCenterWorld;
        }

        protected Vector2 GetTargetAnchoredPosition(RectTransform centerBlock, Vector3 viewportCenterWorld)
        {
            Vector3 targetCenterWorld = GetWorldCenter(centerBlock);
            Vector3 deltaWorld = targetCenterWorld - viewportCenterWorld;
            Vector3 deltaLocal = _content.InverseTransformVector(deltaWorld);
            Vector2 newAnchoredPos = _content.anchoredPosition - (Vector2)deltaLocal;

            return newAnchoredPos;
        }
        protected Vector3 GetWorldCenter(RectTransform rect)
        {
            rect.GetWorldCorners(_corners);
            return (_corners[0] + _corners[2]) * 0.5f;
        }

        #region Debug

        private void OnDrawGizmos()
        {
            if (!_isDebugEnable)
                return;

            if (_content == null || _viewport == null)
                return;

            Vector3 viewportCenter = GetWorldCenter(_viewport) + (Vector3)_offset;
            Gizmos.color = Color.red;
            DrawCross(viewportCenter, 1f);

            if (_content.childCount > 0)
            {
                Gizmos.color = Color.blue;
                foreach (RectTransform child in _content)
                {
                    DrawCross(GetWorldCenter(child), 0.5f);
                }
            }
        }

        private static void DrawCross(Vector3 pos, float size)
        {
            Gizmos.DrawLine(pos + Vector3.left * size, pos + Vector3.right * size);
            Gizmos.DrawLine(pos + Vector3.up * size, pos + Vector3.down * size);
        }
        #endregion
    }
}
