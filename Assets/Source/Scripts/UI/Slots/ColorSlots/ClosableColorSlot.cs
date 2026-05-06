using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public abstract class ClosableColorSlot : ColorSlot
    {
        [Header("Components")]
        [SerializeField] private GameObject _closeGroup;
        [SerializeField] private GameObject _view;

        private new async void OnEnable()
        {
            base.OnEnable();
            await UniTask.Yield();
            bool isOpened = IsOpened();
            _closeGroup.SetActive(!isOpened);
        }

        protected virtual void OpenPreview()
        {
            OnOpenPreview?.Invoke(ColorContext);
        }

        protected abstract bool IsOpened();

        protected override bool CanSelect()
        {
            if (IsOpened())
            {
                return true;
            }
            else
            {
                OpenPreview();
                return false;
            }
        }
    }
}
