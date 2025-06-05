using System;
using System.Collections.Generic;
using Core.MVP.Presenters;
using Core.MVP.Views;
using Game.Data.Settings;
using Game.MVP.Presentation.Presenters;
using Game.MVP.Presentation.Views;
using UnityEngine;

namespace Game.MVP.Presentation.Services
{
    public class LevelsCreateService : IDisposable
    {
        private readonly LevelSettings _settings;
        private readonly LevelService _levelService;
        private readonly SaveLoadService _saveLoadService;
        private readonly MoneyService _moneyService;
        private readonly LevelView.Factory _factory;
        
        private Queue<ViewBase> _levels;
        
        public LevelsCreateService(GameSettings gameSettings, 
            LevelView.Factory factory,
            LevelService levelService, 
            SaveLoadService saveLoadService,
            MoneyService moneyService)
        {
            _settings = gameSettings.LevelSettings;
            _factory = factory;
            _levelService = levelService;
            _saveLoadService = saveLoadService;
            _moneyService = moneyService;

            _levelService.PrepareNewLevel += AddLevel;
        }
        
        public void Dispose()
        {
            _levelService.PrepareNewLevel -= AddLevel;
        }
        
        public void PrepareLevel()
        {
            _levels = new Queue<ViewBase>();
            
            for (int i = 0; i < _settings.LevelsCount; i++)
            {
                LevelView level = Create(i);
                _levels.Enqueue(level);
            }
            _levelService.InvokePrepareLevel();
        }

        private LevelView Create(int index)
        {
            LevelView levelView = _factory.Create();
            levelView.gameObject.transform.localScale = Vector3.one;
            levelView.transform.position = new Vector3(index*_settings.LevelsOffsetX, 0f, 0f);

            if (index == 0)
            {
                IPresenter presenter = new LevelPresenter(levelView, _levelService, _saveLoadService, _moneyService);
                levelView.Construct(presenter);
            }
            
            return levelView;
        }

        private void AddLevel()
        {
            ViewBase levelView = _levels.Dequeue();
            
            levelView.RemovePresenter();
            levelView.Destroy();
            
            
            levelView = _levels.Peek();
            IPresenter presenter = new LevelPresenter((LevelView)levelView, _levelService, _saveLoadService, _moneyService);
            levelView.Construct(presenter);
            
            int index = _levels.Count+1;
            ViewBase newLevel = Create(index);
            _levels.Enqueue(newLevel);
            
            _levelService.InvokePrepareLevel();
        }

        
    }
}