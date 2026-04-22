using Cysharp.Threading.Tasks;
using SanderSaveli.UDK.UI;
using System.Collections.Generic;
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
        private IColorManager _colorManager;
        private ColorGroup _activeGroup;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(IColorManager colorManager, SignalBus signalBus)
        {
            _colorManager = colorManager;
            _signalBus = signalBus;
        }

        private void Start()
        {
            List<ColorSlot> colorSlots = new List<ColorSlot>();
            foreach (var group in _colorGroups)
            {
                group.Init();
                colorSlots.AddRange(group.ColorSlots);
            }
            _radioGroup.SetButtons(colorSlots, _colorManager.ActiveSheme.Value);
        }

        protected override async void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _radioGroup.OnValueChanged += UpdateSelection;
            _colorGroup.OnValueChanged += ChangeActiveGroup;

            await UniTask.Yield();
            foreach (var colorGroup in _colorGroups)
            {
                foreach (ColorSlot item in colorGroup.ColorSlots)
                {
                    item.OnOpenPreview += OnOpenPreview;
                }
                if (colorGroup.HasColorInGroup(_colorManager.ActiveSheme.Value, out _))
                {
                    _activeGroup = colorGroup;
                    _colorGroup.SetSelect(colorGroup.Type);
                    colorGroup.Show();
                }
                else
                {
                    colorGroup.HideImmediately();
                }
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

        private void UpdateSelection(ColorSheme colorSheme)
        {
            _colorManager.ActiveSheme.Value = colorSheme;
        }

        private void ChangeActiveGroup(ColorGroupType type)
        {
            if (_activeGroup == null || _activeGroup.Type != type)
            {
                _activeGroup.Hide();
                _activeGroup = _colorGroups.Find(c => c.Type == type);
                _activeGroup.Show();
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
    }
}
