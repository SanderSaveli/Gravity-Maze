using System;
using UnityEngine;

namespace SanderSaveli.UDK.UI
{
    public abstract class RadioButton<TValue> : MonoBehaviour
    {
        [SerializeField] protected TValue _value;

        public TValue Value => _value;
        public Action<RadioButton<TValue>> OnSelectInput { get; set; }
        public abstract void Select();
        public abstract void Deselect();
    }
}
