using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(Button))]
    public abstract class DisabledButton : MonoBehaviour, ISelectable
    {
        public Button Button => _button;
        public bool IsSelected { get; private set; }

        [SerializeField] protected Button _button;

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        public void Deselect()
        {
            IsSelected = false;
            DisableButton();
        }

        public void Select()
        {
            IsSelected = true;
            EnableButton();
        }

        public void SwitchButton(bool isEnable)
        {
            if(isEnable)
            {
                Select();
            }
            else
            {
                Deselect();
            }
        }

        protected abstract void DisableButton();
        protected abstract void EnableButton();
    }
}
