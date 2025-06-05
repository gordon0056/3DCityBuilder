using System;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Data;
using Game.Data.Dto;
using Game.Data.Enums;
using Game.Data.Settings;
using UnityEngine;
using Zenject;

namespace Game.MVP.Presentation.Services
{
    public class SaveLoadService : IInitializable
    {
        private readonly string _fileName;
        private readonly GameSettings _gameSettings;

        private string _filePath;
        private SaveDto _dto;
        private bool _isLoadingFinish = true;

        public SaveLoadService(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _fileName = _gameSettings.JsonsSettings.SaveJsonName;
        }

        public bool IsLoadingFinish => _isLoadingFinish;
        public SaveDto Dto => _dto;

        public void Initialize()
        {
            _filePath = Application.persistentDataPath + "/" + _fileName;
        }

        public async UniTask TryLoadDataAsync(Action<CallbackType> callback)
        {
            _isLoadingFinish = false;

            if (!File.Exists(_filePath))
            {
                CreateNewDto();
                callback?.Invoke(CallbackType.Loading);
                _isLoadingFinish = true;
                return;
            }

            _dto = JsonUtility.FromJson<SaveDto>(await File.ReadAllTextAsync(_filePath));

            if (_dto == null)
            {
                CreateNewDto();
            }

            callback?.Invoke(CallbackType.Loading);
            _isLoadingFinish = true;
        }
        
        public void SaveMoney(int value, bool isPlus)
        {
            if (isPlus)
                _dto.Money += value;
            else
                _dto.Money -= value;
            
            SaveData();
        }

        public void BuyMagazine(MagazineType type)
        {
            _dto.AllBoughtMagazines++;
            var magazine = _dto.MagazinesInfo.FirstOrDefault(item => item.Type.ToString() == type.ToString());
            if (magazine != null)
            {
                magazine.IsBought = true;
            }
            SaveData();
        }

        public void SaveCurrentParts(MagazineType type, int count)
        {
            var magazine = _dto.MagazinesInfo.FirstOrDefault(item => item.Type.ToString() == type.ToString());
            if (magazine != null)
            {
                magazine.CurrentBuildParts = count;
            } 
            SaveData();
        }

        public void SaveFinishedLevel()
        {
            _dto.CurrentLevelFinished = true;
            SaveData();
        }

        public void SaveCompleteBuild(MagazineType type)
        {
            _dto.AllCompletedBuilds++;
            var magazine = _dto.MagazinesInfo.FirstOrDefault(item => item.Type.ToString() == type.ToString());
            if (magazine != null)
            {
                magazine.CurrentBuildParts = 0;
                magazine.CompletedBuilds++;
            }
            SaveData();
        }

        public void UpdateDtoForNewLevel()
        {
            _dto.CurrentLevelFinished = false;
            
            foreach (MagazineInfo magazineInfo in _dto.MagazinesInfo)
            {
                magazineInfo.CurrentBuildParts = 0;
                magazineInfo.CompletedBuilds = 0;
                magazineInfo.IsBought = false;
            }

            SaveData();
        }

        private void CreateNewDto()
        {
            MagazineInfo[] magazineInfo = new MagazineInfo[]
            {
                new MagazineInfo(MagazineType.Left),
                new MagazineInfo(MagazineType.Right)
            };

            _dto = new SaveDto
            {
                AllCompletedBuilds = 0,
                Money = _gameSettings.MoneySettings.StartMoney,
                MagazinesInfo = magazineInfo,
                CurrentLevelFinished = false
            };

            SaveData();
        }

        private void SaveData()
        {
            File.WriteAllText(_filePath, JsonUtility.ToJson(_dto)); ;
        }
    }
}