using SanderSaveli.UDK.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class SettingsScreen : UiScreen
    {
        [SerializeField] private TMP_InputField _gravityInputField;
        [SerializeField] private TMP_InputField _frictionInputField;
        [SerializeField] private TMP_InputField _bouncinessInputField;
        [SerializeField] private TMP_InputField _rotationInputField;
        private IGameplayConfig _gameplayConfig;

        [Inject]
        public void Construct(IGameplayConfig gameplayConfig)
        {
            _gameplayConfig = gameplayConfig;
        }

        protected override void SubscribeToEvents()
        {
            _gravityInputField.text = _gameplayConfig.GravityForce.ToString();
            _gravityInputField.onValueChanged.AddListener(OnGravityChange);

            _frictionInputField.text = _gameplayConfig.Friction.ToString();
            _frictionInputField.onValueChanged.AddListener(OnFrictionChange);

            _bouncinessInputField.text = _gameplayConfig.Bounciness.ToString();
            _bouncinessInputField.onValueChanged.AddListener(OnBouncinessChange);

            _rotationInputField.text = _gameplayConfig.RotationSpeed.ToString();
            _rotationInputField.onValueChanged.AddListener(OnRotationChange);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _gravityInputField.onValueChanged.RemoveListener(OnGravityChange);
            _frictionInputField.onValueChanged.RemoveListener(OnFrictionChange);
            _bouncinessInputField.onValueChanged.RemoveListener(OnBouncinessChange);
            _rotationInputField.onValueChanged.RemoveListener(OnRotationChange);
            base.UnsubscribeFromEvents();
        }


        private void OnFrictionChange(string value)
        {
            if (float.TryParse(value, out float result))
            {
                _gameplayConfig.Friction = result;
                Debug.Log("Friction: " +  result);
            }
        }
        private void OnBouncinessChange(string value)
        {
            if (float.TryParse(value, out float result))
            {
                _gameplayConfig.Bounciness = result;
                Debug.Log("Bounciness: " + result);
            }
        }
        private void OnRotationChange(string value)
        {
            if (float.TryParse(value, out float result))
            {
                _gameplayConfig.RotationSpeed = result;
                Debug.Log("Rotation: " + result);
            }
        }
        private void OnGravityChange(string value)
        {
            if (float.TryParse(value, out float result))
            {
                _gameplayConfig.GravityForce = result;
                Debug.Log("Gravity: " + result);
            }
        }
    }
}
