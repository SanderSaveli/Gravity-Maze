using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class DeleteAllButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private DataManager _dataManager;

        [Inject]
        public void Construct(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(DeleteData);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(DeleteData);
        }

        private void DeleteData()
        {
            _dataManager.DeleteAllData();
        }
    }
}
