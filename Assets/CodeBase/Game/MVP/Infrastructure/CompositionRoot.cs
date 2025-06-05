using Core.Extensionst;
using Core.Infrastructure.Composition;
using Cysharp.Threading.Tasks;
using Game.MVP.Presentation.Presenters;
using Game.MVP.Presentation.Views;
using UnityEngine;
using Zenject;

namespace Game.MVP.Infrastructure
{
    public class CompositionRoot : SceneCompositionRoot
    {
        [SerializeField] private SceneContext _sceneContext;
        
        private DiContainer _sceneContainer;
        
        public override UniTask Initialize(DiContainer diContainer)
        {
            _sceneContext.Run();
            _sceneContainer = _sceneContext.Container;

            _sceneContainer
                .ConstructView<MainUiView, MainUiPresenter>()
                .ConstructView<PlayerView, PlayerPresenter>()
                .ConstructView<LoadingView, LoadingPresenter>();
            
            return default;
        }
    }
}