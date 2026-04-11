using SanderSaveli.UDK.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public abstract class ClosableColorSlot : ColorRadioButton
    {
        [Header("Components")]
        [SerializeField] private GameObject _closeGroup;
        [SerializeField] private GameObject _view;

        private new void OnEnable()
        {
            bool isOpened = IsOpened();
            _view.SetActive(isOpened);
            _closeGroup.SetActive(!isOpened);
            base.OnEnable();
        }

        protected abstract void OpenPreview();

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
