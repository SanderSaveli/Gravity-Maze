using SanderSaveli.UDK.UI;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LevelsScreen : UiScreen
    {
        public IReadOnlyList<LevelsFiller> Pages => _pages;
        public int OnePageSlotCount => _onePageSlotCount;

        [Header("Components")]
        [SerializeField] private Transform _levelsParent;

        [Header("Prefabs")]
        [SerializeField] private LevelsFiller _levelsFillerPrefab;

        [Header("Params")]
        [SerializeField] private int _onePageSlotCount;

        private List<LevelData> _levelsDatas;
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

        private void Start()
        {
            _levelsDatas = CreateLevelData();
            CreateAllPages();
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

        private List<LevelData> CreateLevelData()
        {
            List<LevelData> levelsDatas = new List<LevelData>();

            int i = 1;
            foreach (ILevelProvider level in _levelManager.Levels)
            {
                levelsDatas.Add(CreateLevelData(i));
                i++;
            }
            levelsDatas[_levelManager.CurrentLevel].Status = LevelStatus.Current;
            levelsDatas[_levelManager.CurrentLevel].StarCount = 0;
            return levelsDatas;
        }

        private LevelData CreateLevelData(int number)
        {
            LevelData data = new LevelData();
            data.Number = number;
            data.StarCount = 3;
            data.Status = (number - 1) <= _levelManager.CurrentLevel ? LevelStatus.Complete : LevelStatus.Locked;
            return data;
        }

        private void CreateAllPages()
        {
            _pages = new List<LevelsFiller>();
            List<List<LevelData>> pages = PaginateList(_levelsDatas);
            foreach (var page in pages)
            {
                _pages.Add(CreatePage(page));
            }
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
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadGame));
        }
    }
}
