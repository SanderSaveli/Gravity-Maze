using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ColorFiller : MonoBehaviour
    {
        [SerializeField] private ColorSlot _alwaysInlockColorSlot;
        [SerializeField] private ColorSlot _byAdsColorSlot;
        [SerializeField] private ColorSlot _byStarColorSlot;
        private DiContainer _container;

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _container = diContainer;
        }

        public List<ColorSlot> Fill(List<ColorContext> colorContexts, Transform parent)
        {
            List<ColorSlot> colors = new List<ColorSlot>();

            foreach (var item in colorContexts)
            {
                colors.Add(CreateSlot(item, parent));
            }
            return colors;
        }

        private ColorSlot CreateSlot(ColorContext colorContext, Transform parent)
        {
            ColorSlot prefab = GetPrefabOfType(colorContext.Type);
            ColorSlot slot = _container.InstantiatePrefabForComponent<ColorSlot>(prefab, parent);
            slot.Fill(colorContext);
            return slot;
        }

        private ColorSlot GetPrefabOfType(ColotUnlockType unlockType)
        {
            switch (unlockType)
            {
                case ColotUnlockType.always:
                    return _alwaysInlockColorSlot;
                case ColotUnlockType.byStar:
                    return _byStarColorSlot;
                case ColotUnlockType.byAds:
                    return _byAdsColorSlot;
                default:
                    throw new System.NotImplementedException($"There is no case for {nameof(ColotUnlockType)} = {unlockType}");
            }
        }
    }
}
