using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class CharSlot : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_Text _firstNumber;
        [SerializeField] private TMP_Text _secondNumber;

        [Header("Params")]
        [SerializeField] private float _animationDuration;
        [SerializeField] private float _offset;

        private Vector2 _targetAncoredPosition;
        private Vector2 _startAncoredPosition;
        private Vector2 _endAncoredPosition;
        private RectTransform _firstRectTransform;
        private RectTransform _secondRectTransform;

        private void Awake()
        {
            _firstRectTransform = _firstNumber.GetComponent<RectTransform>();
            _secondRectTransform = _secondNumber.GetComponent<RectTransform>();

            _targetAncoredPosition = _secondRectTransform.anchoredPosition;

            _startAncoredPosition = _targetAncoredPosition;
            _startAncoredPosition.y += _offset;

            _endAncoredPosition = _targetAncoredPosition;
            _endAncoredPosition.y -= _offset;
        }

        public void SetNumberWithoutAnimation(char number, char secondNumber =' ')
        {
            _firstNumber.text = number.ToString();
            _secondNumber.text = secondNumber.ToString();

            _firstRectTransform.anchoredPosition = _targetAncoredPosition;
            _secondRectTransform.anchoredPosition = _startAncoredPosition;
            _firstNumber.alpha = 1;
            _secondNumber.alpha = 0;
        }

        public async UniTask PlayAnimation(char firstNumber, char secondNumber)
        {
            _firstRectTransform.DOKill();
            _secondRectTransform.DOKill();
            _firstNumber.DOKill();
            _secondNumber.DOKill();

            SetNumberWithoutAnimation(firstNumber, secondNumber);

            Sequence sequence = DOTween.Sequence();

            sequence
                .Append(_firstRectTransform.DOAnchorPos(_endAncoredPosition, _animationDuration))
                .Join(_firstNumber.DOFade(0, _animationDuration))
                .Join(_secondRectTransform.DOAnchorPos(_targetAncoredPosition, _animationDuration))
                .Join(_secondNumber.DOFade(1, _animationDuration));

            await sequence.AsyncWaitForCompletion();
        }
    }
}
