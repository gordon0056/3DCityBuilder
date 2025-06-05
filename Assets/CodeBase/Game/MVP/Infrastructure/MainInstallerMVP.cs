using Game.Data.Settings;
using Game.Elements;
using Game.MVP.Presentation.Managers;
using Game.MVP.Presentation.Presenters;
using Game.MVP.Presentation.Services;
using Game.MVP.Presentation.Views;
using UnityEngine;
using Zenject;

namespace Game.MVP.Infrastructure
{
    public class MainInstallerMVP : MonoInstaller
    {
        [SerializeField] private GameSettings _settings;

        [SerializeField] private StoneElement _stoneElement;
        [SerializeField] private int _stonePoolCount;
        
        [SerializeField] private MainUiView _mainUiView;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private LevelView _levelView;
        [SerializeField] private LoadingView _loadingView;

        public override void InstallBindings()
        {
            BindSettings();
            BindPools();
            BindFactories();
            BindServices();
            BindViews();
            BindPresenters();
            BindManagers();
        }

        private void BindSettings()
        {
            Container.BindInstance(_settings);
        }

        private void BindPools()
        {
            Container
                .BindMemoryPool<StoneElement, StoneElement.Pool>()
                .WithInitialSize(_stonePoolCount)
                .FromComponentInNewPrefab(_stoneElement)
                .UnderTransformGroup("Stone Elements Pool");
        }

        private void BindFactories()
        {
            Container
                .BindFactory<LevelView, LevelView.Factory>()
                .FromComponentInNewPrefab(_levelView);
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<SaveLoadService>().AsSingle(); 
            Container.BindInterfacesAndSelfTo<MoneyService>().AsSingle(); 
            Container.Bind<LevelService>().AsSingle(); 
            Container.Bind<TimerService>().AsSingle();
            Container.Bind<LevelsCreateService>().AsSingle(); 
        }

        private void BindViews()
        {
            Container.BindInstance(_mainUiView);
            Container.BindInstance(_playerView);
            Container.BindInstance(_loadingView);
        }

        private void BindPresenters()
        {
            Container.BindInterfacesAndSelfTo<MainUiPresenter>().AsTransient();
            Container.BindInterfacesAndSelfTo<PlayerPresenter>().AsTransient();
            Container.Bind<LoadingPresenter>().AsTransient();
        }

        private void BindManagers()
        {
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        }
    }
}