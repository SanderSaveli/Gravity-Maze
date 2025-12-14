using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SanderSaveli.UDK
{
    public class TimerTestView : MonoBehaviour, IPoolableObject<TimerTestView>
    {
        [SerializeField] private TMP_Text _idLabel;
        [SerializeField] private TMP_Text _timeLabel;
        [SerializeField] private Button _cancelButton;

        private TimerHandle _handle;

        public Action<TimerTestView> OnBackToPool { get; set; }

        private void Awake()
        {
            _cancelButton.onClick.AddListener(Cancel);
        }

        public void Bind(int id, TimerHandle handle)
        {
            _handle = handle;
            _idLabel.text = $"{id}";
        }

        public void UpdateTime(float remaining)
        {
            var ts = TimeSpan.FromSeconds(remaining);
            _timeLabel.text = $"{ts.Minutes:D2}:{ts.Seconds:D2}:{ts.Milliseconds:D3}";
        }

        public void OnActive()
        {
            // —брос состо€ни€ при вз€тии из пула
            _timeLabel.text = "";
        }

        private void Cancel()
        {
            _handle.Cancel();
            OnBackToPool?.Invoke(this);
        }
    }
}
