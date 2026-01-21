using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class GameButtonView : MonoBehaviour
    {
        [SerializeField] private GameButton _gameButton;
        [Space]
        [SerializeField] private float _demping = 0.5f;
        [SerializeField] private float _animationDuration = 0.4f;
 
        private void OnEnable()
        {
            _gameButton.OnActive += HandleAnimate;
        }

        private void OnDisable()
        {
            _gameButton.OnActive -= HandleAnimate;
        }

        private void HandleAnimate()
        {
            Vector3 nextPos;
            nextPos = transform.position;
            nextPos -= transform.up * _demping;
            transform.DOLocalMove(nextPos, _animationDuration);
        }
    }
}
