using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Animations
{
    public class FillAnimation : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private Image _image;

        private TweenerCore<float, float, FloatOptions> _tween;

        public async UniTask DoFillAnimationAsync(float value, CancellationToken token, UnityAction callback = null)
        {
            _tween = _image.DOFillAmount(value, _duration);

            await _tween.WithCancellation(token);
            
            callback?.Invoke();
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }
    }
}