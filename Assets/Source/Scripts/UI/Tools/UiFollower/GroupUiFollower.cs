using Cysharp.Threading.Tasks;
using SanderSaveli.UDK.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class GroupUiFollower<T> : UITargetFollower where T : Enum
    {
        [SerializeField] private RadioButtonGroup<T> _colorGroup;

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

        private void NewVarianSelect(T value)
        {
            RectTransform target = _colorGroup.ActiveElement.GetComponent<RectTransform>();
            MoveTo(target);
        }
    }
}
