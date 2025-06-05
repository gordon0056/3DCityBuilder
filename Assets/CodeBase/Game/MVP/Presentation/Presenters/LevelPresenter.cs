using System.Linq;
using Core.MVP.Presenters;
using Game.Data.Dto;
using Game.Data.Enums;
using Game.Elements;
using Game.MVP.Presentation.Services;
using Game.MVP.Presentation.Views;

namespace Game.MVP.Presentation.Presenters
{
    public class LevelPresenter : IPresenter
    {
        private readonly LevelView _view;
        private readonly LevelService _levelService;
        private readonly SaveLoadService _saveLoadService;
        private readonly MoneyService _moneyService;

        public LevelPresenter(
            LevelView view, 
            LevelService levelService, 
            SaveLoadService saveLoadService,
            MoneyService moneyService)
        {
            _view = view;
            _levelService = levelService;
            _saveLoadService = saveLoadService;
            _moneyService = moneyService;

            _moneyService.OpenMagazine += OpenMagazine;

            _levelService.PrepareLevel += PrepareLevel;
            _levelService.BuildItem += BuildPart;
            _levelService.PrepareStonesProgress += PrepareStonesProgress;
            Subscribe();
        }
        
        public void Enable()
        {
            
        }

        public void Disable()
        {
            _moneyService.OpenMagazine -= OpenMagazine;
            
            _levelService.PrepareLevel -= PrepareLevel;
            _levelService.BuildItem -= BuildPart;
            _levelService.PrepareStonesProgress -= PrepareStonesProgress;
            
            Unsubscribe();
        }

        private void Subscribe()
        {
            foreach (MagazineElement magazine in _view.MagazineElements)
            {
                magazine.InvokeShowMagazineProgress += ShowMagazineProgress;
                magazine.InvokeStonesProgress += UpdateStonesProgress;
                magazine.InvokeFinishBuild += SaveFinishedBuild;
            }
        }

        private void Unsubscribe()
        {
            foreach (MagazineElement magazine in _view.MagazineElements)
            {
                magazine.InvokeShowMagazineProgress -= ShowMagazineProgress;
                magazine.InvokeStonesProgress -= UpdateStonesProgress;
                magazine.InvokeFinishBuild -= SaveFinishedBuild;
            }
        }

        private void PrepareLevel()
        {
            var magazine =
                _view.MagazineElements.FirstOrDefault(magazineLeft => magazineLeft.Type == MagazineType.Left);
            if(magazine!=null)
                magazine.Init(_saveLoadService.Dto.MagazinesInfo.FirstOrDefault(infoLeft => infoLeft.Type == MagazineType.Left));
            
            magazine = 
                _view.MagazineElements.FirstOrDefault(magazineRight => magazineRight.Type == MagazineType.Right);
            if(magazine!=null)
                magazine.Init(_saveLoadService.Dto.MagazinesInfo.FirstOrDefault(infoRight => infoRight.Type == MagazineType.Right));
            
            _view.BlockObject.SetActive(!_saveLoadService.Dto.CurrentLevelFinished);
        }

        private void BuildPart(int id)
        {
            foreach (MagazineElement magazine in _view.MagazineElements)
            {
                magazine.BuildPart(id);
            }
        }

        private void ShowMagazineProgress(MagazineProgressDto dto, bool isAnimation)
        {
            _levelService.InvokeShowProgressBar(dto,isAnimation);
            if (_view.MagazineElements.All(item => item.Status == BuildingStatus.Builded))
            {
                _saveLoadService.SaveFinishedLevel();
                _view.BlockObject.SetActive(false);
            }
        }

        private void UpdateStonesProgress(MagazineType type, StonesProgressDto dto)
        {
            _levelService.InvokeUpdateStonesProgress(dto);

            if (dto != null)
                _saveLoadService.SaveCurrentParts(type, dto.MaxStones - dto.NeedStones);
            else
                SaveFinishedBuild(type);

        }

        private void SaveFinishedBuild(MagazineType type)
        {
            _saveLoadService.SaveCompleteBuild(type);
        }

        private void PrepareStonesProgress(MagazineType type)
        {
            MagazineElement magazineElement = _view.MagazineElements.FirstOrDefault(item => item.Type == type);
            
            if(magazineElement != null)
                magazineElement.UpdateStonesProgress();
        }

        private void OpenMagazine(MagazineType type)
        {
            MagazineElement magazineElement = _view.MagazineElements.FirstOrDefault(item => item.Type == type);
            
            if(magazineElement != null)
                magazineElement.SetDoorState(false);
        }
    }
}