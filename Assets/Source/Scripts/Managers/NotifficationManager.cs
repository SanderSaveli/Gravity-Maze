using R3;
using SanderSaveli.UDK;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class NotifficationManager : MonoBehaviour, INotifficationManager
    {
        public IEnumerable<ColorSheme> UnshownColors => _unshownColors;


        public ReactiveProperty<bool> HasUnshownColors { get; private set; }

        private List<ColorSheme> _unshownColors;
        private IStorageService _storageService;
        private const string Unshown_key = "Account/Notiffication";

        private void Awake()
        {
            _storageService = new EncryptedJsonToFileStorageService();
            HasUnshownColors = new ReactiveProperty<bool>(false);
            _storageService.Load<List<ColorSheme>>(Unshown_key, OnConfigLoaded);
        }

        private void OnConfigLoaded(List<ColorSheme> colorShemes)
        {
            if (colorShemes == null)
            {
                colorShemes = new List<ColorSheme>();
                _unshownColors = colorShemes;
                Save();
            }
            _unshownColors = colorShemes;
            HasUnshownColors.Value = _unshownColors.Count > 0;
        }

        public void OnAllColorsShowed()
        {
            _unshownColors.Clear();
            Save();
        }

        public void OnColorShowed(ColorSheme colorSheme)
        {
            _unshownColors.Remove(colorSheme);
            Save();
        }

        public void UnlockNewColor(ColorSheme colorSheme)
        {
            _unshownColors.Insert(0, colorSheme);
            HasUnshownColors.Value = _unshownColors.Count > 0;
        }

        private void Save()
        {
            _storageService.Save(Unshown_key, _unshownColors);
            HasUnshownColors.Value = _unshownColors.Count > 0;
        }

    }
}
