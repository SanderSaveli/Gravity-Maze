using SanderSaveli.UDK.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class DebugSettingsScreen : UiScreen
    {
        [SerializeField] private TMP_InputField _gravityInputField;
        [SerializeField] private Slider _frictionInputField;
        [SerializeField] private Slider _bouncinessInputField;
        [SerializeField] private TMP_InputField _rotationInputField;
        [SerializeField] private SpeedRadioButtonGroup _speedRadioButtonGroup;
        private IGameplayConfig _gameplayConfig;
        private IAppSettings _appSettings;

        [Inject]
        public void Construct(IGameplayConfig gameplayConfig, IAppSettings appSettings)
        {
            _gameplayConfig = gameplayConfig;
            _appSettings = appSettings;
        }

        protected override void SubscribeToEvents()
        {
            _gravityInputField.text = _gameplayConfig.GravityForce.ToString();
            _gravityInputField.onValueChanged.AddListener(OnGravityChange);

            _frictionInputField.value = _gameplayConfig.Friction;
            _frictionInputField.onValueChanged.AddListener(OnFrictionChange);

            _bouncinessInputField.value = _gameplayConfig.Bounciness;
            _bouncinessInputField.onValueChanged.AddListener(OnBouncinessChange);

            _rotationInputField.text = _gameplayConfig.RotationSpeed.ToString();
            _rotationInputField.onValueChanged.AddListener(OnRotationChange);

            _speedRadioButtonGroup.OnValueChanged += HandleChangeTimeMode;
            _speedRadioButtonGroup.SetSelect(_appSettings.TimeMode.Value);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _gravityInputField.onValueChanged.RemoveListener(OnGravityChange);
            _frictionInputField.onValueChanged.RemoveListener(OnFrictionChange);
            _bouncinessInputField.onValueChanged.RemoveListener(OnBouncinessChange);
            _rotationInputField.onValueChanged.RemoveListener(OnRotationChange);
            _speedRadioButtonGroup.OnValueChanged -= HandleChangeTimeMode;
            base.UnsubscribeFromEvents();
        }


        private void OnFrictionChange(float value)
        {
            _gameplayConfig.Friction = value;
            Debug.Log("Friction: " + value);
        }
        private void OnBouncinessChange(float value)
        {
            _gameplayConfig.Bounciness = value;
            Debug.Log("Bounciness: " + value);
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

        private void HandleChangeTimeMode(TimeMode timeMode)
        {
            _appSettings.TimeMode.Value = timeMode;
            Debug.Log(_appSettings.TimeMode.Value);
        }
    }
}
