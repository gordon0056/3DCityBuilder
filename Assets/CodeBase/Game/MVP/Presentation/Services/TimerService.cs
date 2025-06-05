using System;
using Cysharp.Threading.Tasks;
using Game.Data.Enums;
using Game.Data.Settings;

namespace Game.MVP.Presentation.Services
{
    public class TimerService
    {
        private readonly float _duration;
        
        private bool _isTimerFinish = true;

        public TimerService(GameSettings gameSettings)
        {
            _duration = gameSettings.JsonsSettings.LoadDuration;
        }
        public bool IsTimerFinish => _isTimerFinish;

        public async UniTask StartTimerAsync(Action<CallbackType> callback)
        {
            _isTimerFinish = false;

            await UniTask.Delay(TimeSpan.FromSeconds(_duration));

            _isTimerFinish = true;
            callback?.Invoke(CallbackType.Timer);
        }
    }
}