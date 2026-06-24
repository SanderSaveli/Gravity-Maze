using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class AudioDebugButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }


        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OpenAudioDebug);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OpenAudioDebug);
        }

        private void OpenAudioDebug()
        {
            _signalBus.Fire(new SignalInputOpenMenuPopup(UDK.MenuPopupType.AudioDebug));
        }
    }
}
