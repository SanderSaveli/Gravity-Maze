using System;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public abstract class SelectButton : MonoBehaviour, ISelectable
    {
        [SerializeField] protected Button _button;

        public bool IsSelected { get; protected set; }
        public Action<bool> OnSwitched { get; set; }

        private void OnEnable()
        {
            if (IsSelected)
            {
                HandleSelect();
            }
            else
            {
                HandleDeselect();
            }
            _button.onClick.AddListener(HandleSwitch);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleSwitch);
        }

        private void HandleSwitch()
        {
            if (IsSelected)
            {
                Deselect();
            }
            else
            {
                Select();
            }
        }

        public void Select()
        {
            if (IsSelected == true)
                return;

            IsSelected = true;
            OnSwitched?.Invoke(IsSelected);
            HandleSelect();
        }

        public void Deselect()
        {
            if (IsSelected == false)
                return;

            IsSelected = false;
            OnSwitched?.Invoke(IsSelected);
            HandleDeselect();
        }

        public void SetState(bool state)
        {
            if (state)
            {
                Select();
            }
            else
            {
                Deselect();
            }
        }

        protected abstract void HandleSelect();
        protected abstract void HandleDeselect();
    }
}
