using SanderSaveli.UDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class TutorialManager : MonoBehaviour, ITutorialManager
    {
        [SerializeField] private List<LevelTutorialData> _tutorialDatas;
        private IStorageService _storageService;
        private HashSet<int> _completedTutorials;
        private const string TUTORIAL_SAVE_PATH = "Save/Tutorials";

        private void Start()
        {
            _storageService = new EncryptedJsonToFileStorageService();
            _storageService.Load<HashSet<int>>(TUTORIAL_SAVE_PATH, LoadTutorials);
        }

        public bool HasTutorialForLevel(int level, out LevelTutorialData data)
        {
            data = _tutorialDatas.FirstOrDefault(x => x.level == level);

            if(_completedTutorials == null)
            {
                return true;
            }
            if (_completedTutorials.Contains(level))
            {
                return false;
            }
            return data != null;
        }

        public void CompleteTutorial(int level)
        {
            _completedTutorials.Add(level);
            _storageService.Save(TUTORIAL_SAVE_PATH, _completedTutorials);
        }

        private void LoadTutorials(HashSet<int> completedTutorials)
        {
            if(completedTutorials == null)
            {
                completedTutorials = new HashSet<int>();
            }
            _completedTutorials = completedTutorials;
        }
    }

    [Serializable]
    public class LevelTutorialData
    {
        public int level;
        public string tytorialKey;
    }
}
