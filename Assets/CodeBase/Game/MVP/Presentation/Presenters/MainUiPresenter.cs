using System;
using System.Linq;
using System.Threading;
using Core.Infrastructure.WindowsFsm;
using Core.MVP.Presenters;
using Cysharp.Threading.Tasks;
using Game.Data.Dto;
using Game.Data.Enums;
using Game.MVP.Presentation.Services;
using Game.MVP.Presentation.Views;
using Game.Shared.Windows;
using UnityEngine;
using Zenject;

namespace Game.MVP.Presentation.Presenters
{
    public class MainUiPresenter : IPresenter, ILateTickable
    {
        private readonly LevelService _levelService;
        private readonly MoneyService _moneyService;
        private readonly SaveLoadService _saveLoadService;
        private readonly MainUiView _view;
        private readonly IWindowFsm _windowFsm;
        private readonly Type _window = typeof(MainUi);

        private CancellationTokenSource _cts;
        private Transform _uiStoneTarget;
        private Transform _uiLeftMoneyTarget;
        private Transform _uiRightMoneyTarget;

        public MainUiPresenter(
            MainUiView view, 
            LevelService levelService, 
            MoneyService moneyService,
            SaveLoadService saveLoadService,
            IWindowFsm windowFsm)
        {
            _view = view; 
            _levelService = levelService;
            _moneyService = moneyService;
            _saveLoadService = saveLoadService;
            _windowFsm = windowFsm;

            _moneyService.UpdateMoney += UpdateMoneyCounter;
            _moneyService.ShowHaveNotMoney += ShowHaveNotMoneyMessage;
            _moneyService.OpenMagazine += UpdateMagazine;

            _levelService.UpdateStonesCount += UpdateStonesCount;
            _levelService.ShowProgressBar += ShowProgressBarAsync;
            _levelService.UpdateStonesProgress += UpdateStonesProgress;
            _levelService.ShowStonesProgress += ShowStonesProgress;
            _levelService.ShowMoneyPrices += UpdateMoneyPrices;
        }
        
        public void Enable()
        {
            _windowFsm.Opened += OnHandleOpenWindow;
            _windowFsm.Closed += OnHandleCloseWindow;
        }

        public void Disable()
        {
            _windowFsm.Opened -= OnHandleOpenWindow;
            _windowFsm.Closed -= OnHandleCloseWindow;
            
            _moneyService.UpdateMoney -= UpdateMoneyCounter;
            _moneyService.ShowHaveNotMoney -= ShowHaveNotMoneyMessage;
            _moneyService.OpenMagazine -= UpdateMagazine;
            
            _levelService.UpdateStonesCount -= UpdateStonesCount;
            _levelService.ShowProgressBar -= ShowProgressBarAsync;
            _levelService.UpdateStonesProgress -= UpdateStonesProgress;
            _levelService.ShowStonesProgress -= ShowStonesProgress;
            _levelService.ShowMoneyPrices -= UpdateMoneyPrices;
            
            _cts?.Dispose();
            _cts = null;
        }
        
        public void LateTick()
        {
            if (_uiStoneTarget != null && _view.StoneProgressPoint.gameObject.activeSelf)
            {
                CalculatePosition(_uiStoneTarget.position, _view.StoneProgressPoint);
            }
            
            if (_uiLeftMoneyTarget != null && _view.LeftMoneyPoint.gameObject.activeSelf)
            {
                CalculatePosition(_uiLeftMoneyTarget.position, _view.LeftMoneyPoint);
            }
            
            if (_uiRightMoneyTarget != null && _view.RightMoneyPoint.gameObject.activeSelf)
            {
                CalculatePosition(_uiRightMoneyTarget.position, _view.RightMoneyPoint);
            }
        }

        private void CalculatePosition(Vector3 targetPosition, RectTransform pointTransform)
        {
            Vector3 position = _view.MainCamera.WorldToScreenPoint(targetPosition);

            float offset = pointTransform.sizeDelta.x / 2;
            position.x = Mathf.Clamp(position.x, offset, Screen.width - offset);
            position.y = Mathf.Clamp(position.y, offset, Screen.height - offset);
            
            pointTransform.position = position;
        }

        private void OnHandleOpenWindow(Type window)
        {
            if(_window != window || _view == null) return;
            
            _view.StonesCounterObject.SetActive(false);
            _view.ProgressBarObject.SetActive(false);
            _view.Show();
        }
        
        private void OnHandleCloseWindow(Type window)
        {
            if(_window != window || _view == null) return;
            
            _view.Hide();
        }

        private void UpdateStonesCount(int count)
        {
            _view.StonesCounterObject.SetActive(count > 0);
            _view.StonesCountText.text = count.ToString();
        }

        private void UpdateMoneyCounter(int value)
        {
            _view.MoneyCountText.text = value.ToString();
        }

        private async void ShowProgressBarAsync(MagazineProgressDto dto, bool isAnimation)
        {
            _view.ProgressBarText.text = $"{dto.Current}/{dto.Max}";
            _view.ProgressBarObject.SetActive(dto.IsActive);
            float fill = (float) dto.Current / dto.Max;
            
            if (isAnimation)
            {
                try
                {
                    _cts = new CancellationTokenSource();
                    await _view.FillAnimation.DoFillAnimationAsync(fill, _cts.Token);
                }
                catch (OperationCanceledException e)
                {
                    _view.ProgressBarFillImage.fillAmount = fill;
                    Debug.Log($"Operation Cancelled:\n{e}");
                }
                finally
                {
                    _cts?.Dispose();
                    _cts = null;
                }
            }
            _view.ProgressBarFillImage.fillAmount = fill;
        }

        private void UpdateStonesProgress(StonesProgressDto dto)
        {
            if (dto == null)
            {
                ShowStonesProgress(false);
                return;
            }
            
            _view.StoneProgressText.text = dto.NeedStones.ToString();
            _uiStoneTarget = dto.Target;
        }

        private void UpdateMoneyPrices(MoneyPriceDto dto)
        {
            if (dto == null || !dto.IsShow)
            {
                _view.LeftMoneyPoint.gameObject.SetActive(false);
                _view.RightMoneyPoint.gameObject.SetActive(false);
                return;
            }
            
            _uiLeftMoneyTarget = dto.LeftTransform;
            _uiRightMoneyTarget = dto.RightTransform;

            UpdateMagazine();
        }

        private void UpdateMagazine(MagazineType type = default)
        {
            int price;
            var magazineInfo = _saveLoadService.Dto.MagazinesInfo.FirstOrDefault(item => item.Type == MagazineType.Left);
            
            if(_moneyService.GetMagazinePrice(MagazineType.Left, out price) && magazineInfo!= null && !magazineInfo.IsBought)
            {
                _view.LeftMoneyPoint.gameObject.SetActive(true);
                _view.LeftMoneyText.text = price.ToString();
            }
            else
            {
                _view.LeftMoneyPoint.gameObject.SetActive(false);
                _uiLeftMoneyTarget = null;
            }
            
            magazineInfo = _saveLoadService.Dto.MagazinesInfo.FirstOrDefault(item => item.Type == MagazineType.Right);

            if (_moneyService.GetMagazinePrice(MagazineType.Right, out price) && magazineInfo != null && !magazineInfo.IsBought)
            {
                _view.RightMoneyPoint.gameObject.SetActive(true);
                _view.RightMoneyText.text = price.ToString();
            }
            else
            {
                _view.RightMoneyPoint.gameObject.SetActive(false);
                _uiRightMoneyTarget = null;
            }
        }

        private void ShowStonesProgress(bool isActive, MagazineType type = default)
        {
            var info = _saveLoadService.Dto.MagazinesInfo.FirstOrDefault(item => item.Type == type);
            if(info!=null && info.CompletedBuilds >=2 && isActive)
                return;
            _view.StoneProgressPoint.gameObject.SetActive(isActive);
        }

        private async void ShowHaveNotMoneyMessage()
        {
            try
            {
                _cts = new CancellationTokenSource();
                _view.HaveNotMoneyText.gameObject.SetActive(true);
                await UniTask.Delay(TimeSpan.FromSeconds(2), false, PlayerLoopTiming.Update, _cts.Token);
            }
            catch (OperationCanceledException e)
            {
                Debug.Log($"Operation Cancelled:\n{e}");
            }
            finally
            {
                _view.HaveNotMoneyText.gameObject.SetActive(false);
                _cts?.Dispose();
                _cts = null;
            }
        }
    }
}
