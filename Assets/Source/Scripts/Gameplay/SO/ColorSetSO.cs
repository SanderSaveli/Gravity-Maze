using CustomText;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [CreateAssetMenu(fileName = "new Color Set", menuName = "GravityMaze/Color Set")]
    public class ColorSetSO : ScriptableObject
    {
        [SerializeField] private List<ColorParams> _colors;

        public List<ColorParams> Colors => _colors;

        private void OnEnable()
        {
            if (_colors == null || _colors.Count == 0)
            {
                InitializeIfNeeded();
            }
        }

        private void OnValidate()
        {
            _colors.ForEach(c => c.Name = c.TextColorType.ToString());
        }

        private void InitializeIfNeeded()
        {
            _colors = new List<ColorParams>();

            foreach (Custom_ColorStyle color in Enum.GetValues(typeof(Custom_ColorStyle)))
            {
                ColorParams colorParams = new ColorParams();
                colorParams.TextColorType = color;
                colorParams.Color = Color.white;
                colorParams.Name = color.ToString();
                _colors.Add(colorParams);
            }
        }
    }
}
