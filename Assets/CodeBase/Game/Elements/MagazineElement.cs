using System;
using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.Data.Dto;
using Game.Data.Enums;
using UnityEngine;

namespace Game.Elements
{
    public class MagazineElement : MonoBehaviour
    {
        [SerializeField] private MagazineType _type;
        [SerializeField] private List<BuildingElement> _buildingElements;
        [SerializeField] private GameObject _doorObject;

        private BuildingStatus _status = BuildingStatus.NotBuilded;
        private BuildingElement _currentBuildingElement;

        public MagazineType Type => _type;
        public BuildingStatus Status => _status;
        public event Action<MagazineProgressDto, bool> InvokeShowMagazineProgress;
        public event Action<MagazineType, StonesProgressDto> InvokeStonesProgress;
        public event Action<MagazineType> InvokeFinishBuild;

        private void Awake()
        {
            SubscribeBuildingElements();
        }

        private void OnDestroy()
        {
            UnsubscribeBuildingElements();
        }

        public void Init(MagazineInfo info)
        {
            foreach (BuildingElement buildingElement in _buildingElements)
            {
                buildingElement.Init();
                buildingElement.gameObject.SetActive(false);
            }
            
            SetDoorState(!info.IsBought);
            for (int i = 0; i < _buildingElements.Count; i++)
            {
                if (i < info.CompletedBuilds)
                {
                    _buildingElements[i].gameObject.SetActive(true);
                    _buildingElements[i].SetBuildingStatus(BuildingStatus.Builded);
                }
                else
                {
                    _buildingElements[i].gameObject.SetActive(true);
                    _buildingElements[i].SetBuildedParts(info.CurrentBuildParts);
                    _currentBuildingElement = _buildingElements[i];
                    return;
                }
            }

            if (_buildingElements.All(item => item.Status == BuildingStatus.Builded))
            {
                _currentBuildingElement = null;
                _status = BuildingStatus.Builded;
            }
        }

        public void BuildPart(int id)
        {
            BuildingElement buildingElement = _buildingElements.FirstOrDefault(item => item.ID == id);
            if (buildingElement != null)
            {
                buildingElement.BuildPart(true);
            }
        }

        public void ShowProgress(bool isActive, bool isAnimation)
        {
            int current = _buildingElements.Count(item => item.IsFullCompleted());
            int max = _buildingElements.Count;
            MagazineProgressDto dto = new MagazineProgressDto(current, max, isActive);
            InvokeShowMagazineProgress?.Invoke(dto, isAnimation);
        }
        
        public void UpdateStonesProgress()
        {
            if (_currentBuildingElement != null)
            {
                int needStones = _currentBuildingElement.NeedStones();
                StonesProgressDto dto = new StonesProgressDto(needStones, 
                    _currentBuildingElement.MaxStones(), 
                    _currentBuildingElement.BuyPlace);
                InvokeStonesProgress?.Invoke(Type, dto);
            }
        }
        
        public void SetDoorState(bool isActive)
        {
            _doorObject.SetActive(isActive);
        }
        
        private void SubscribeBuildingElements()
        {
            foreach (BuildingElement buildingElement in _buildingElements)
            {
                buildingElement.UpdateStoneProgress += UpdateStoneBuildProgress;
            }
        }
        
        private void UnsubscribeBuildingElements()
        {
            foreach (BuildingElement buildingElement in _buildingElements)
            {
                buildingElement.UpdateStoneProgress -= UpdateStoneBuildProgress;
            }
        }
        
        private void UpdateStoneBuildProgress(int allCount, int completedCount)
        {
            if (allCount == completedCount)
            {
                _currentBuildingElement = _buildingElements.FirstOrDefault(item => item.Status == BuildingStatus.NotBuilded);
                if (_currentBuildingElement != null)
                {
                    _currentBuildingElement.gameObject.SetActive(true);
                    InvokeFinishBuild?.Invoke(Type);
                }
                else
                {
                    _status = BuildingStatus.Builded;
                    InvokeStonesProgress?.Invoke(Type, null);
                }
                ShowProgress(true, true);
            }

            UpdateStonesProgress();
        }
    }
}