using Cysharp.Threading.Tasks;
using SanderSaveli.UDK.UI;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class SplashScreen : UiScreen
    {
        [SerializeField] private float _splashScreenDuration;

        protected override void Awake()
        {
            ScreenRect = GetComponent<RectTransform>();
        }

        private async void Start()
        {
            ShowImmediately();
            await UniTask.WaitForSeconds(_splashScreenDuration, true);
            Hide();
        }
    }
}
