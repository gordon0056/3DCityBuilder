using Core.Infrastructure;
using Core.Infrastructure.Composition;
using Core.Infrastructure.WindowsFsm;
using Game.Infrastructure.GameFsm;
using Zenject;

namespace Game.Infrastructure.Installers
{
    public class RootInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<StateFactory>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<GameStateMachine>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<Coroutines>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<SceneLoader>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<SceneInitializer>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<WindowFsm>()
                .AsSingle();
        }
    }
}