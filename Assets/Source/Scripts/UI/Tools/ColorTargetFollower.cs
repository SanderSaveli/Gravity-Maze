using Cysharp.Threading.Tasks;
using System.Collections.Generic;
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
            transform.parent = target;
            _sliderParent = null;
            _colorGroup.OnValueChanged += NewVarianSelect;
            MoveToImmediately(target);
        }

        private void OnDestroy()
        {
            _colorGroup.OnValueChanged -= NewVarianSelect;
        }

        private void NewVarianSelect(ColorSheme value)
        {
            RectTransform target = _colorGroup.ActiveElement.GetComponent<RectTransform>();
            transform.SetParent(target);
            _sliderParent = null;
            MoveTo(target);
        }
    }
}
