using System;
using Core.Infrastructure.WindowsFsm;
using Game.Data.Dto;
using Game.Data.Enums;
using Game.Shared.Windows;

namespace Game.MVP.Presentation.Services
{
    public class LevelService
    {
        private readonly IWindowFsm _windowFsm;
        private readonly SaveLoadService _saveLoadService;

        public LevelService(IWindowFsm windowFsm, SaveLoadService saveLoadService)
        {
            _windowFsm = windowFsm;
            _saveLoadService = saveLoadService;
        }
        
        public event Action PrepareLevel;
        public event Action PrepareNewLevel;
        public event Action<int> UpdateStonesCount;
        public event Action<int> BuildItem;
        public event Action<MagazineProgressDto, bool> ShowProgressBar;
        public event Action<StonesProgressDto> UpdateStonesProgress;
        public event Action<bool, MagazineType> ShowStonesProgress;
        public event Action<MagazineType> PrepareStonesProgress;
        public event Action<MoneyPriceDto> ShowMoneyPrices;

        public void InvokePrepareLevel()
        {
            _windowFsm.OpenWindow(typeof(MainUi), false);
            PrepareLevel?.Invoke();
        }
        
        public void InvokerPrepareNewLevel()
        {
            _saveLoadService.UpdateDtoForNewLevel();
            PrepareNewLevel?.Invoke();
        }

        public void InvokeUpdateStonesCount(int count)
        {
            UpdateStonesCount?.Invoke(count);
        }

        public void InvokeBuildItem(int id)
        {
            BuildItem?.Invoke(id);
        }

        public void InvokeShowProgressBar(MagazineProgressDto dto, bool isAnimation)
        {
            ShowProgressBar?.Invoke(dto, isAnimation);
        }

        public void InvokeUpdateStonesProgress(StonesProgressDto dto)
        {
            UpdateStonesProgress?.Invoke(dto);
        }

        public void InvokeShowStonesProgress(bool isActive, MagazineType type)
        {
            if(isActive)
                PrepareStonesProgress?.Invoke(type);
            
            ShowStonesProgress?.Invoke(isActive, type);
        }

        public void InvokeShowMoneyPrices(MoneyPriceDto dto)
        {
            ShowMoneyPrices?.Invoke(dto);
        }
    }
}