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
        [SerializeField] private TMP_Text _levelNumber;
        [SerializeField] private Transform _starParent;
        [SerializeField] private Image _lockImage;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Button _button;

        [Header("Prefabs")]
        [SerializeField] private GameObject _starPrefab;

        [Header("Params")]
        [SerializeField] private Color _lockColor = Color.white;
        [SerializeField] private Color _currentColor = Color.white;
        [SerializeField] private Color _completeColor = Color.white;

        private LevelData _levelData;
        private List<GameObject> _stars = new();
        private ILevelManager _levelManager;

        [Inject]
        public void Construct(ILevelManager levelManager)
        {
            _levelManager = levelManager;
        }


        private void OnEnable()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        public void Fill(LevelData value)
        {
            _levelData = value;
            _levelNumber.text = value.Number.ToString();
            switch (value.Status)
            {
                case LevelStatus.Complete:
                    _lockImage.gameObject.SetActive(false);
                    _backgroundImage.color = _completeColor;
                    SetStarCount(value.StarCount);
                    break;
                case LevelStatus.Current:
                    _lockImage.gameObject.SetActive(false);
                    _backgroundImage.color = _currentColor;
                    SetStarCount(value.StarCount);
                    break;
                case LevelStatus.Locked:
                    _levelNumber.text = "";
                    _lockImage.gameObject.SetActive(true);
                    _backgroundImage.color = _lockColor;
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
            Debug.Log("Click");
            if(_levelData.Status != LevelStatus.Locked)
            {
                OnSelectLevel?.Invoke(_levelData);
            }
        }
    }
}
