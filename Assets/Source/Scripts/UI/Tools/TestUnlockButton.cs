using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class TestUnlockButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private UnlockColorScreen _unlockColorPopup;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleTest);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleTest);
        }

        private void HandleTest()
        {

            _unlockColorPopup.Init(ColorSheme.dark_1);
            _unlockColorPopup.gameObject.SetActive(true);
            _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.OpenColor));
        }
    }
}
