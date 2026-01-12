using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class ColorTargetFollower : UITargetFollower
    {
        [SerializeField] private ColorRadioGroup _colorGroup;

        private new async void Start()
        {
            base.Start();
            await UniTask.Yield();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_sliderParent);
            await UniTask.Yield();
            RectTransform target = _colorGroup.ActiveElement.GetComponent<RectTransform>();
            MoveToImmediately(target);
        }

        private void OnEnable()
        {
            _colorGroup.OnValueChanged += NewVarianSelect;
        }

        private void OnDisable()
        {
            _colorGroup.OnValueChanged -= NewVarianSelect;
        }

        private void NewVarianSelect(ColorSheme value)
        {
            Debug.Log(value.ToString());
            RectTransform target = _colorGroup.ActiveElement.GetComponent<RectTransform>();
            MoveTo(target);
        }
    }
}
