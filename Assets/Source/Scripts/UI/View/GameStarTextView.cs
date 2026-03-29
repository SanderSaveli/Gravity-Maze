using TMPro;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class GameStarTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private ILevelStorage _levelStorage;

        [Inject]
        public void Construct(ILevelStorage levelStorage)
        {
            _levelStorage = levelStorage;
        }

        public void OnEnable()
        {
            _text.text = _levelStorage.StarCount.ToString();
        }
    }
}
