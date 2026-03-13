using SanderSaveli.UDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class AudioDebugScreen : PopupScreen
    {
        [SerializeField] private List<ButtonAudioPair> _pairs;
        private Dictionary<Button, UnityAction> _listeners;
        private IAudioManager _audioManager;

        [Inject]
        public void Construct(IAudioManager audioManager)
        {
            _audioManager = audioManager;
            _listeners = new Dictionary<Button, UnityAction>();
        }

        protected override void SubscribeToEvents()
        {
            foreach (var pair in _pairs)
            {
                UnityAction action = () => HandleAudio(pair.AudioClip);
                pair.Button.onClick.AddListener(action);
                _listeners.Add(pair.Button, action);
            }
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            foreach (var pair in _pairs)
            {
                if (_listeners.TryGetValue(pair.Button, out var action))
                {
                    pair.Button.onClick.RemoveListener(action);
                }
            }

            _listeners.Clear();
        }

        private void HandleAudio(AudioClip audioClip)
        {
            _audioManager.ChangeMainAudio(audioClip);
        }

        [Serializable]
        private class ButtonAudioPair
        {
            public Button Button;
            public AudioClip AudioClip;
        }
    }
}
