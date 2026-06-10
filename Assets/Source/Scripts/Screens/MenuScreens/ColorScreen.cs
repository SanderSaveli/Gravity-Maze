using Cysharp.Threading.Tasks;
using SanderSaveli.UDK.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ColorScreen : UiScreen
    {
        [SerializeField] private ColorRadioGroup _radioGroup;
        [SerializeField] private List<ColorGroup> _colorGroups;
        [SerializeField] protected ColorGroupRadioGroup _colorGroup;
        [SerializeField] private ColorByAdsScreen _colorByAdsScreen;
        [SerializeField] private ColorByStarScreen _colorByStarsScreen;
        [SerializeField] private SelectingSnapScroll _selectingSnapScroll;

        private IColorManager _colorManager;
        private ColorGroup _activeGroup;
        private SignalBus _signalBus;
        private bool _isColorsInited;
        private INotifficationManager _notifficationManager;
        private List<ColorSlot> _colorSlots;

        [Inject]
        public void Construct(IColorManager colorManager, SignalBus signalBus, INotifficationManager notifficationManager)
        {
            _colorManager = colorManager;
            _signalBus = signalBus;
            _notifficationManager = notifficationManager;
        }

        private void Start()
        {
            _colorSlots = new List<ColorSlot>();
            foreach (var group in _colorGroups)
            {
                group.Init();
                _colorSlots.AddRange(group.ColorSlots);
            }
            _isColorsInited = true;
            _radioGroup.SetButtons(_colorSlots, _colorManager.ActiveSheme.Value);
            SubscribeToPreviewEvents();
        }

        protected async override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _radioGroup.OnValueChanged += UpdateSelection;
            _colorGroup.OnValueChanged += ChangeActiveGroup;

            if (_isColorsInited)
            {
                SubscribeToPreviewEvents();
            }

            await UniTask.Yield();
            Canvas.ForceUpdateCanvases();
            ScrollToActive();
            if (_notifficationManager.HasUnshownColors.Value)
            {
                ScrollToLastAdded();
                NotifyAll();
            }
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _radioGroup.OnValueChanged -= UpdateSelection;
            _colorGroup.OnValueChanged -= ChangeActiveGroup;
            foreach (var colorGroup in _colorGroups)
            {
                foreach (ColorSlot item in colorGroup.ColorSlots)
                {
                    item.OnOpenPreview -= OnOpenPreview;
                }
            }
        }

        private void ScrollToActive()
        {
            ColorSheme activeSheme = _colorManager.ActiveSheme.Value;
            ScroolTo(activeSheme, true);
        }

        private void ScrollToLastAdded()
        {
            ColorSheme activeSheme = _notifficationManager.UnshownColors.First();
            ScroolTo(activeSheme, false);
        }

        private void ScroolTo(ColorSheme sceme, bool isForceSelect)
        {
            foreach (var colorGroup in _colorGroups)
            {
                if (colorGroup.HasColorInGroup(sceme, out _))
                {
                    _activeGroup = colorGroup;
                    if(isForceSelect)
                    {
                        _colorGroup.SetSelect(colorGroup.Type);
                    }
                    colorGroup.Select();
                }
                else
                {
                    colorGroup.Deselect();
                }
            }
            if (isForceSelect)
            {
                _radioGroup.SetSelect(sceme);
            }
            _selectingSnapScroll.SnapToWithoutSelection(_activeGroup.transform as RectTransform);
        }

        private void UpdateSelection(ColorSheme colorSheme)
        {
            _colorManager.ActiveSheme.Value = colorSheme;
        }

        private void ChangeActiveGroup(ColorGroupType type)
        {
            if (_activeGroup == null || _activeGroup.Type != type)
            {
                _activeGroup.Deselect();
                _activeGroup = _colorGroups.Find(c => c.Type == type);
                _activeGroup.Select();
            }
        }

        private void SubscribeToPreviewEvents()
        {
            foreach (var colorGroup in _colorGroups)
            {
                foreach (ColorSlot item in colorGroup.ColorSlots)
                {
                    item.OnOpenPreview += OnOpenPreview;
                }
            }
        }

        private void OnOpenPreview(ColorContext colorContext)
        {
            switch (colorContext.Type)
            {
                case ColotUnlockType.always:
                    break;
                case ColotUnlockType.byStar:
                    _colorByStarsScreen.Init(colorContext);
                    _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.ColorByStars));
                    break;
                case ColotUnlockType.byAds:
                    _colorByAdsScreen.Init(colorContext);
                    _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.ColorByAds));
                    break;
                default:
                    break;
            }
        }

        private void NotifyAll()
        {
            foreach (var item in _notifficationManager.UnshownColors)
            {
                _colorSlots.Find(t => t.Value == item).Notify();
            }
            _notifficationManager.OnAllColorsShowed();
        }
    }
}
