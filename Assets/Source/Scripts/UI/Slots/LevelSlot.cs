using CustomText;
using Cysharp.Threading.Tasks;
using SanderSaveli.UDK.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LevelSlot : MonoBehaviour, ISlot<LevelData>
    {
        public Action<LevelData> OnSelectLevel {  get; set; }

        [Header("Components")]
        [SerializeField] private CustomText.CustomText _levelNumber;
        [SerializeField] private Transform _starParent;
        [SerializeField] private Image _lockImage;
        [SerializeField] private Image _border;
        [SerializeField] private ImageColorByType _backgroundImage;
        [SerializeField] private Button _button;

        [Header("Prefabs")]
        [SerializeField] private GameObject _starPrefab;

        [Header("Params")]
        [SerializeField] private Custom_ColorStyle _lockColor;
        [SerializeField] private Custom_ColorStyle _currentColor;
        [SerializeField] private Custom_ColorStyle _completeColor;
        [Space]
        [SerializeField] private Custom_ColorStyle _defaultTextColor;
        [SerializeField] private Custom_ColorStyle _activeTextColor;

        private LevelData _levelData;
        private List<GameObject> _stars = new();

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        public async void Fill(LevelData value)
        {
            _levelData = value;
            _levelNumber.text = value.Number.ToString();
            switch (value.Status)
            {
                case LevelStatus.Complete:
                    _lockImage.gameObject.SetActive(false);
                    _levelNumber.ChangeColor(_defaultTextColor);
                    _backgroundImage.ChangeColor(_completeColor);
                    _border.gameObject.SetActive(true);
                    SetStarCount(value.StarCount);
                    break;
                case LevelStatus.Current:
                    _lockImage.gameObject.SetActive(false);
                    _levelNumber.ChangeColor(_activeTextColor);
                    _backgroundImage.ChangeColor(_currentColor);
                    _border.gameObject.SetActive(false);
                    SetStarCount(value.StarCount);
                    break;
                case LevelStatus.Locked:
                    _levelNumber.text = "";
                    _lockImage.gameObject.SetActive(true);
                    _backgroundImage.ChangeColor(_lockColor);
                    _border.gameObject.SetActive(false);
                    SetStarCount(0);
                    break;
            }
        }

        private void SetStarCount(int count)
        {
            int i = 0;
            for (; i < count; i++)
            {
                if (_stars.Count <= i)
                {
                    _stars.Add(Instantiate(_starPrefab, _starParent));
                }
                _stars[i].gameObject.SetActive(true);
            }
            for (; i < _stars.Count; i++)
            {
                _stars[i].gameObject.SetActive(false);
            }
        }

        private void HandleButtonClick()
        {
            if(_levelData.Status != LevelStatus.Locked)
            {
                OnSelectLevel?.Invoke(_levelData);
            }
        }
    }
}
