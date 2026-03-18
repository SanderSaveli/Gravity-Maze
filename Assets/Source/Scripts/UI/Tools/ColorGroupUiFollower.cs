using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class ColorGroupUiFollower : UITargetFollower
    {
        [SerializeField] private ColorGroupRadioGroup _colorGroup;

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

        private void NewVarianSelect(ColorGroupType value)
        {
            Debug.Log("New variant");
            RectTransform target = _colorGroup.ActiveElement.GetComponent<RectTransform>();
            MoveTo(target);
        }
    }
}
