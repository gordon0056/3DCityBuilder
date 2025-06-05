using System;
using Core.Infrastructure.WindowsFsm;
using Core.MVP.Presenters;
using Game.MVP.Presentation.Views;
using Game.Shared.Windows;

namespace Game.MVP.Presentation.Presenters
{
    public class LoadingPresenter : IPresenter
    {
        private readonly IWindowFsm _windowFsm;
        private readonly LoadingView _view;
        private readonly Type _window = typeof(Loading);

        public LoadingPresenter(IWindowFsm windowFsm, LoadingView view)
        {
            _windowFsm = windowFsm;
            _view = view;
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
        }

        private void OnHandleOpenWindow(Type window)
        {
            if(_window != window || _view == null) return;
            
            _view.Show();
            _view.Animation.DoRotateAnimation();
        }
        
        private void OnHandleCloseWindow(Type window)
        {
            if(_window != window || _view == null) return;
            
            _view.Hide();
        }
    }
}