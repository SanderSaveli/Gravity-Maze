using UnityEngine;
using Zenject;

namespace SanderSaveli.UDK
{
    public class TextManagerInstaller : MonoInstaller
    {
        [SerializeField] private TestTextManager _textManager;

        public override void InstallBindings()
        {
            Container.Bind<ITextManager>().FromInstance(_textManager).AsSingle().NonLazy();
            Container.Bind<ILanguageChanger<LanguageType>>().FromInstance(_textManager).AsSingle().NonLazy();
        }
    }
}
