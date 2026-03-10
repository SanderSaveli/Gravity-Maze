using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class PressFreezeView : MonoBehaviour
    {
        [SerializeField] private PressFreeze _pressFreze;
        [SerializeField] private GameObject _area;

        private void OnEnable()
        {
            _pressFreze.OnStatusChange += ChangeArea;
        }
        private void OnDisable()
        {
            _pressFreze.OnStatusChange -= ChangeArea;
        }

        private void ChangeArea(bool isOn)
        {
            _area.SetActive(isOn);
        }
    }
}
