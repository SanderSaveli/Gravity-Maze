using DG.Tweening;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class GameButtonTransformHandler : GameButtonHandler
    {
        [Header("Properties")]
        [SerializeField] private Vector3 _position;
        [SerializeField] private float _rotation;
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private Ease _ease;

        protected override void HandleGameButtonAction()
        {
            transform
                .DOLocalMove(_position, _animationDuration)
                .SetEase(_ease)
                .SetLink(gameObject);

            Vector3 roteteVec = new Vector3(0, 0, _rotation);
            transform
                .DOLocalRotate(roteteVec, _animationDuration)
                .SetEase(_ease)
                .SetLink(gameObject);
        }

        public void Record()
        {
            _position = transform.localPosition;
            _rotation = transform.localRotation.eulerAngles.z;
        }
    }
}
