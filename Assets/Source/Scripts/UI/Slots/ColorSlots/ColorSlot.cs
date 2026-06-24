using SanderSaveli.UDK.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public abstract class ColorSlot : ColorRadioButton, ISlot<ColorContext>
    {
        public Action<ColorContext> OnOpenPreview { get; set; }
        public ColorContext ColorContext { get; private set; }
        [SerializeField] private List<Image> _activeColorElements;
        [SerializeField] private WaveSpawner _waveSpawner;
        protected IColorManager _colorManager;

        [Inject]
        public void Construct(IColorManager colorManager)
        {
            _colorManager = colorManager;
        }

        public virtual void Fill(ColorContext value)
        {
            _value = value.ColorSheme;
            ColorContext = value;
            Color activeColor = _colorManager.GetActiveColorOfSheme(_value);
            foreach (var element in _activeColorElements)
            {
                Color col = element.color;
                element.color = new Color(activeColor.r, activeColor.g, activeColor.b, col.a);
            }
        }

        public void Notify()
        {
            Color activeColor = _colorManager.GetActiveColorOfSheme(_value);
            _waveSpawner.SetWaveColor(activeColor);
            _waveSpawner.StartSpawn();
        }

        public override void Select()
        {
            _waveSpawner.StopSpawn();
            base.Select();
        }
    }
}
