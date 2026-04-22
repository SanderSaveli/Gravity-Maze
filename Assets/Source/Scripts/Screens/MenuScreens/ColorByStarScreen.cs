using System;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ColorByStarScreen : ClosableUIScreen
    {
        [SerializeField] private TMP_Text _starText;
        [SerializeField] private Button _previewButton;
        [SerializeField] private Image _previewImage;
        [SerializeField] private WaveSpawner _waveSpawner;
        [SerializeField] private string _format = "{0}/{1}";

        private ILevelStorage _levelStorage;
        private IColorManager _colorManager;
        private ColorSheme _colorSheme;
        private bool _isPreviewActive;

        [Inject]
        public void Construct(ILevelStorage levelStorage, IColorManager colorManager)
        {
            _levelStorage = levelStorage;
            _colorManager = colorManager;
        }

        public void Init(int needCount, ColorSheme colorSheme)
        {
            _colorSheme = colorSheme;
            _starText.text = string.Format(_format, _levelStorage.StarCount, needCount);
            _previewImage.color = _colorManager.GetActiveColorOfSheme(_colorSheme);
            _waveSpawner.SetWaveColor(_colorManager.GetActiveColorOfSheme(colorSheme));
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _previewButton.onClick.AddListener(HandlePreview);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _previewButton.onClick.RemoveListener(HandlePreview);
        }

        public override void Hide(Action callback = null)
        {
            if (_isPreviewActive)
            {
                _colorManager.ShowActiveSheme();
                _isPreviewActive = false;
            }
            base.Hide(callback);
        }

        private void HandlePreview()
        {
            if(!_isPreviewActive)
            {
                _colorManager.PreviewSheme(_colorSheme);
                _isPreviewActive = true;
            }
            else
            {
                _colorManager.ShowActiveSheme();
                _isPreviewActive = false;
            }
        }
    }
}
