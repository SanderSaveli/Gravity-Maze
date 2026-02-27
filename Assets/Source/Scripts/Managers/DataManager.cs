using SanderSaveli.UDK;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class DataManager : MonoBehaviour
    {
        public ILevelStorage LevelStorage => _accountStorage;
        private AccountStorage _accountStorage = new AccountStorage();
        private AccountData _accountData;
        private IStorageService _storageService;
        private const string ACCOUNT_SAVE_PATH = "Save/Account";
        private ILevelManager _levelManager;

        [Inject]
        public void Construct(ILevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        private void Awake()
        {
            _storageService = new JsonToFileStorageService();
            _storageService.Load<AccountData>(ACCOUNT_SAVE_PATH, OnDataLoaded);
        }

        private void OnDataLoaded(AccountData accountData)
        {
            bool isNeedSave= false;
            if (accountData == null)
            {
                _accountData = new AccountData();
                isNeedSave = true;
            }
            else
            {
                _accountData = accountData;
            }
            if (_levelManager.Levels.Count != _accountData.levels.Count)
            {
                _accountData.levels = CreateLevelData(_accountData.levels);
            }

            _accountStorage.SetData(_accountData);

            _accountStorage.OnUpdate += Save;
            if(isNeedSave)
            {
                Save();
            }
        }

        public void Save()
        {
            if (_accountStorage == null)
                return;

            _accountData = _accountStorage.GetActualData();
            _storageService.Save(ACCOUNT_SAVE_PATH, _accountData);
        }

        private List<LevelSaveData> CreateLevelData(List<LevelSaveData> currData)
        {
            List<LevelSaveData> levelsDatas = new List<LevelSaveData>();

            int i = 0;
            foreach (ILevelProvider level in _levelManager.Levels)
            {
                int starCount = 0;
                if(i < currData.Count)
                {
                    starCount = currData[i].star_count;
                }
                LevelSaveData data = new LevelSaveData(starCount);
                levelsDatas.Add(data);
                i++;
            }
            return levelsDatas;
        }
    }
}
