using Cysharp.Threading.Tasks;
using SanderSaveli.UDK;
using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class TutorialView : UiScreen
    {
        [SerializeField] private TextByTableKey _textByTableKey;
        [SerializeField] private float _showTime = 10;
        [SerializeField] private float _delay = 1f;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _signalBus.Subscribe<SignalGameEnd>(HandleGameEnd);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _signalBus.Unsubscribe<SignalGameEnd>(HandleGameEnd);
        }

        public async UniTask ShowTutorial(LevelTutorialData levelTutorialData)
        {
            var token = this.GetCancellationTokenOnDestroy();

            _textByTableKey.ChangeText(levelTutorialData.tytorialKey);

            await UniTask.Delay(System.TimeSpan.FromSeconds(_delay), cancellationToken: token);

            Show();

            await UniTask.Delay(System.TimeSpan.FromSeconds(_showTime), cancellationToken: token);

            Hide();
        }
        private void HandleGameEnd()
        {
            Hide();
        }
    }
}
