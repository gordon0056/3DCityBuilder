using Core.Infrastructure;
using Core.Infrastructure.Composition;
using Core.Infrastructure.GameFsm.States;
using Core.Infrastructure.WindowsFsm;
using Cysharp.Threading.Tasks;
using Game.Shared.Windows;

namespace Game.Infrastructure.GameFsm.States
{
    public class MainGameState : IState
    {
        private const string Scene = "MainScene";
        private readonly SceneLoader _sceneLoader;
        private readonly SceneInitializer _sceneInitializer;
        private readonly IWindowResolve _windowResolve;

        public MainGameState(
            SceneLoader sceneLoader,
            SceneInitializer sceneInitializer,
            IWindowResolve windowResolve)
        {
            _sceneLoader = sceneLoader;
            _sceneInitializer = sceneInitializer;
            _windowResolve = windowResolve;
        }

        public void OnExit()
        {
            
        }

        public void OnEnter()
        {
            PreparedWindows();

            _sceneLoader.Load(Scene, OnLoaded);
        }

        private void OnLoaded()
        {
            _sceneInitializer.Initialize().Forget();
        }

        private void PreparedWindows()
        {
            _windowResolve.CleanUp();

            _windowResolve.Set<Loading>();
            _windowResolve.Set<MainUi>();
        }
    }
}