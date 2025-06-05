using Core.Infrastructure.WindowsFsm;
using Cysharp.Threading.Tasks;
using Game.Data.Enums;
using Game.MVP.Presentation.Services;
using Game.Shared.Windows;
using Zenject;

namespace Game.MVP.Presentation.Managers
{
    public class GameManager : IInitializable
    {
        private readonly IWindowFsm _windowFsm;
        private readonly LevelsCreateService _levelsCreateService;
        private readonly SaveLoadService _saveLoadService;
        private readonly TimerService _timerService;
        private readonly MoneyService _moneyService;

        public GameManager(
            IWindowFsm windowFsm, 
            LevelsCreateService levelsCreateService, 
            SaveLoadService saveLoadService,
            TimerService timerService,
            MoneyService moneyService)
        {
            _windowFsm = windowFsm;
            _levelsCreateService = levelsCreateService;
            _saveLoadService = saveLoadService;
            _timerService = timerService;
            _moneyService = moneyService;
        }
        
        public void Initialize()
        {
            _windowFsm.OpenWindow(typeof(Loading), true);
            _timerService.StartTimerAsync(OnHandleCallback).Forget();
            _saveLoadService.TryLoadDataAsync(OnHandleCallback).Forget();
        }

        private void OnHandleCallback(CallbackType callbackType)
        {
            if (callbackType == CallbackType.Loading && _timerService.IsTimerFinish ||
                callbackType == CallbackType.Timer && _saveLoadService.IsLoadingFinish)
            {
                _windowFsm.CloseWindow(typeof(Loading));
                _windowFsm.OpenWindow(typeof(MainUi), true);
                _levelsCreateService.PrepareLevel();
                _moneyService.Init();
            }
        }
    }
}