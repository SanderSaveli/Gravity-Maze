using TMPro;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fpsText;
        [SerializeField] private float _updateInterval = 0.5f;

        private float _timeLeft;
        private int _frames;
        private float _fps;

        private void Awake()
        {
            _timeLeft = _updateInterval;
        }

        private void Update()
        {
            _timeLeft -= Time.unscaledDeltaTime;
            _frames++;

            if (_timeLeft <= 0f)
            {
                _fps = _frames / _updateInterval;
                _fpsText.text = Mathf.RoundToInt(_fps).ToString();

                _timeLeft = _updateInterval;
                _frames = 0;
            }
        }
    }
}

