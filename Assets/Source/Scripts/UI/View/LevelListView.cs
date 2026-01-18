using Cysharp.Threading.Tasks;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class LevelListView : MonoBehaviour
    {
        [SerializeField] private LevelsScreen _levelScreen;
        [SerializeField] private SelectingSnapScroll _scroll;
        [SerializeField] private DisabledButton _previousButton;
        [SerializeField] private DisabledButton _nextButton;
        [SerializeField] private TMP_Text _levelsText;
        [Space]
        [SerializeField] private string _format = "{0}/{1}";

        private LevelsFiller _currentFiller;

        private async void OnEnable()
        {
            await UniTask.Yield();
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            if(_levelScreen.Pages == null)
                return;

            foreach (var page in _levelScreen.Pages)
            {
                page.OnLevelListSelected += UpdateView;
                if (page.IsSelected)
                {
                    UpdateView(page);
                }
            }

            _nextButton.Button.onClick.AddListener(HandleNext);
            _previousButton.Button.onClick.AddListener(HandlePrevious);
        }

        private void UnsubscribeFromEvents()
        {
            if (_levelScreen.Pages == null)
                return;

            foreach (var page in _levelScreen.Pages)
            {
                page.OnLevelListSelected -= UpdateView;
            }

            Debug.Log(_nextButton);
            Debug.Log(_nextButton.Button);
            _nextButton.Button.onClick.RemoveListener(HandleNext);
            _previousButton.Button.onClick.RemoveListener(HandlePrevious);
        }

        private void UpdateView(LevelsFiller filler)
        {
            _currentFiller = filler;
            _levelsText.text = string.Format(_format, filler.MinLevel, filler.MaxLevel);

            _previousButton.SwitchButton(_levelScreen.Pages.First() != filler);
            _nextButton.SwitchButton(_levelScreen.Pages.Last() != filler);
        }

        private void HandleNext()
        {
            Debug.Log("Next");
            int index = _levelScreen.Pages.ToList().IndexOf(_currentFiller);
            if (index + 1 >= _levelScreen.Pages.Count)
                return;
            Debug.Log("Next_2");
            LevelsFiller filler = _levelScreen.Pages[index+1];
            _scroll.SnapTo(filler.GetComponent<RectTransform>());
        }

        private void HandlePrevious()
        {
            int index = _levelScreen.Pages.ToList().IndexOf(_currentFiller);
            if (index <= 0)
                return;

            LevelsFiller filler = _levelScreen.Pages[index - 1];

            _scroll.SnapTo(filler.GetComponent<RectTransform>());
        }
    }
}
