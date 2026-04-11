using TMPro;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ColorByStarScreen : ClosableUIScreen
    {
        [SerializeField] private TMP_Text _starText;
        [SerializeField] private string _format = "{0}/{1}";

        private ILevelStorage _levelStorage;

        [Inject]
        public void Construct(ILevelStorage levelStorage)
        {
            _levelStorage = levelStorage;
        }

        public void Init(int needCount)
        {
            _starText.text = string.Format(_format, _levelStorage.StarCount, needCount);
        }
    }
}
