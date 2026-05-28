using R3;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class GameStarTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private ILevelStorage _levelStorage;
        private CompositeDisposable _compositeDisposable;

        [Inject]
        public void Construct(ILevelStorage levelStorage)
        {
            _levelStorage = levelStorage;
        }

        public void OnEnable()
        {
            _compositeDisposable = new CompositeDisposable();
            _levelStorage.Levels.Subscribe(UpdateStars).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        private void UpdateStars(List<LevelSaveData> data)
        {
            _text.text = _levelStorage.StarCount.ToString();
        }
    }
}
