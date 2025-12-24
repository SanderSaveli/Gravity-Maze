using System;
using UnityEngine;

namespace SanderSaveli.UDK.UI
{
    public class PauseUIScreen : UiScreen
    {
        public override void Show(Action callback = null)
        {
            base.Show(callback);
            Time.timeScale = 0;
        }

        public override void Hide(Action callback = null)
        {
            base.Hide(callback);
            Time.timeScale = 1;
        }
    }
}
