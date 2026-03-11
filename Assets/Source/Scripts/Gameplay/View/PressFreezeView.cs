using DG.Tweening;
using Google.MiniJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SanderSaveli.GravityMaze
{
    public class PressFreezeView : MonoBehaviour
    {
        [SerializeField] private PressFreeze _pressFreze;
        [SerializeField] private GameObject _area;

        [SerializeField] private float _animationDuration = 0.2f;
        [SerializeField] private Ease _ease;
        private Tween _activeTween;
        private Vector3 _targetScale;

        private void OnEnable()
        {
            _targetScale = _area.transform.localScale;
            _area.transform.localScale = Vector3.zero;
            _pressFreze.OnStatusChange += ChangeArea;
        }

        private void OnDisable()
        {
            _pressFreze.OnStatusChange -= ChangeArea;
        }

        private void ChangeArea(bool isOn)
        {
            if(isOn)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            _activeTween?.Kill();
            _area.SetActive(true);
            _activeTween = 
                _area.transform.DOScale(_targetScale, _animationDuration)
                .SetEase(_ease)
                .OnComplete(() => _activeTween = null);
        }

        private void Hide()
        {
            _activeTween?.Kill();
            _activeTween =
                _area.transform.DOScale(Vector3.zero, _animationDuration)
                .SetEase(_ease)
                .OnComplete(() => 
                { 
                    _activeTween = null;
                    _area.SetActive(false);
                });
        }
    }
}
