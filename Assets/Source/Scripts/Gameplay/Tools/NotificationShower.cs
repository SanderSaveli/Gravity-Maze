using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class NotificationShower : MonoBehaviour
    {
        [SerializeField] private NotifficationView _notifficationView;
        private IColorManager _colorManager;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(IColorManager colorManager, SignalBus signalBus)
        {
            _colorManager = colorManager;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalStarCountIncrease>(CheckToNewColor);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalStarCountIncrease>(CheckToNewColor);
        }

        private void CheckToNewColor(SignalStarCountIncrease ctx)
        {
            ColorContext colorContext = _colorManager.ColorContexts
                .FirstOrDefault(t => t.Type == ColotUnlockType.byStar && t.StarToUnlock == ctx.StarCont);

            if(colorContext != null)
            {
                Color color = _colorManager.GetActiveColorOfSheme(colorContext.ColorSheme);
                _notifficationView.ShowNewColor(color);
            }
        }
    }
}
