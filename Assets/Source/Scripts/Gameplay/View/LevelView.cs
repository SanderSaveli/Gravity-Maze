using SanderSaveli.UDK.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelText;
        private IGameContext _gameContext;

        [Inject]
        public void Construct(IGameContext gameContext)
        {
            _gameContext = gameContext;
        }

        private void Start()
        {
            _levelText.text = GetString(_gameContext.LevelNumber + 1);
        }

        public string GetString(int number)
        {
            string numberStr = (number % (int)Mathf.Pow(10, 3)).ToString();

            return numberStr.PadLeft(3, '0');
        }
    }
}
