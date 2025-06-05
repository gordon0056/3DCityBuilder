using System;
using System.Linq;
using Game.Data.Enums;
using Game.Data.Settings;
using UnityEngine;
using Zenject;

namespace Game.MVP.Presentation.Services
{
    public class MoneyService : ITickable
    {
        private readonly SaveLoadService _saveLoadService;
        private readonly GameSettings _gameSettings;

        private int _currentMoney;
        private float _time;
        private bool _isInited;
        
        public MoneyService(SaveLoadService saveLoadService, GameSettings gameSettings)
        {
            _saveLoadService = saveLoadService;
            _gameSettings = gameSettings;
            
        }

        public event Action<int> UpdateMoney;
        public event Action ShowHaveNotMoney;
        public event Action<MagazineType> OpenMagazine;

        public void Init()
        {
            _currentMoney = _saveLoadService.Dto.Money;
            _time = _gameSettings.MoneySettings.TickSeconds;
            UpdateMoney?.Invoke(_currentMoney);
            _isInited = true;
        }

        public void TryBuyBuild(MagazineType type)
        {
            var magazineInfo = _saveLoadService.Dto.MagazinesInfo.FirstOrDefault(item => item.Type == type);
            if(magazineInfo==null || magazineInfo.IsBought)
                return;
            
            int price = _gameSettings.MoneySettings.BuildingPriceMultiplier *
                        _saveLoadService.Dto.AllBoughtMagazines;
            if (_currentMoney >= price)
            {
                _currentMoney -= price;
                _saveLoadService.SaveMoney(price, false);
                _saveLoadService.BuyMagazine(type);
                OpenMagazine?.Invoke(type);
                UpdateMoney?.Invoke(_currentMoney);
            }
            else
            {
                ShowHaveNotMoney?.Invoke();
            }
        }

        public bool GetMagazinePrice(MagazineType type, out int price)
        {
            bool isAvailable = false;
            price = 0;
            var magazineInfo = _saveLoadService.Dto.MagazinesInfo.First(magazine => magazine.Type == type);
            if (magazineInfo!=null)
            {
                isAvailable = magazineInfo.IsBought;
                price = _gameSettings.MoneySettings.BuildingPriceMultiplier * _saveLoadService.Dto.AllBoughtMagazines;
            }
            return !isAvailable;
        }

        public void Tick()
        {
            if(!_isInited)
                return;

            _time -= Time.deltaTime;

            if (_time <= 0f)
            {
                int value = _gameSettings.MoneySettings.MoneyTickMultiplier * _saveLoadService.Dto.AllCompletedBuilds;
                _currentMoney += value;
                _saveLoadService.SaveMoney(value, true);
                UpdateMoney?.Invoke(_currentMoney);
                _time = _gameSettings.MoneySettings.TickSeconds; 
            }
        }
    }
}