using Cysharp.Threading.Tasks;
using SanderSaveli.UDK.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LevelsScreen : UiScreen
    {
        public IReadOnlyList<LevelsFiller> Pages => _pages;
        public int OnePageSlotCount => _onePageSlotCount;

        [Header("Components")]
        [SerializeField] private Transform _levelsParent;
        [SerializeField] private SelectingSnapScroll _scroll;

        [Header("Prefabs")]
        [SerializeField] private LevelsFiller _levelsFillerPrefab;

        [Header("Params")]
        [SerializeField] private int _onePageSlotCount;

        private List<LevelsFiller> _pages;
        private ILevelManager _levelManager;
        private DiContainer _container;
        private SignalBus _signalBus;
        private IGameContext _gameContext;

        [Inject]
        public void Construct(ILevelManager levelManager, DiContainer container, SignalBus signalBus, IGameContext gameContext)
        {
            _levelManager = levelManager;
            _container = container;
            _signalBus = signalBus;
            _gameContext = gameContext;
        }

        private async void Start()
        {
            CreateAllPages();
            await UniTask.Yield();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_levelsParent as RectTransform);
            await UniTask.Yield();
            OpenPageWithCurrentLevel();
        }

        private new void OnDestroy()
        {
            foreach (var page in _pages)
            {
                foreach (LevelSlot slot in page.Levels)
                {
                    slot.OnSelectLevel -= OnLevelSelect;
                }
            }
            base.OnDestroy();
        }

        private void CreateAllPages()
        {
            _pages = new List<LevelsFiller>();
            List<List<LevelData>> pages = PaginateList(_levelManager.LevelsData.ToList());
            foreach (var page in pages)
            {
                _pages.Add(CreatePage(page));
            }
        }

        private void OpenPageWithCurrentLevel()
        {
            int pageNumber = Mathf.FloorToInt(_levelManager.CurrentLevel / _onePageSlotCount);
            LevelsFiller filler = Pages[pageNumber];

            _scroll.SnapImmediatley(filler.transform as RectTransform);
            Debug.Log("Page Number " + pageNumber);
        }

        private List<List<LevelData>> PaginateList(List<LevelData> allLevels)
        {
            List<List<LevelData>> pages = new List<List<LevelData>>();

            for (int i = 0; i < allLevels.Count; i += _onePageSlotCount)
            {
                int count = Mathf.Min(_onePageSlotCount, allLevels.Count - i);
                pages.Add(allLevels.GetRange(i, count));
            }

            return pages;
        }

        private LevelsFiller CreatePage(List<LevelData> levels)
        {
            LevelsFiller filler = _container.InstantiatePrefabForComponent<LevelsFiller>(_levelsFillerPrefab, _levelsParent);
            filler.FillItems(levels);
            foreach (LevelSlot levelSlot in filler.Levels)
            {
                levelSlot.OnSelectLevel += OnLevelSelect;
            }
            return filler;
        }

        private void OnLevelSelect(LevelData levelData)
        {
            _gameContext.LevelNumber = levelData.Number - 1;
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadLevelFromMenu));
        }
    }
}
