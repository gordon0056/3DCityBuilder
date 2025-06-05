using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Animations
{
    public class MoveAnimation : MonoBehaviour
    {
        [SerializeField] private float _duration;
        
        private TweenerCore<Vector3, Vector3, VectorOptions> _tween;
        
        public async UniTask DoMoveAnimationAsync(Vector3 position, CancellationToken token, UnityAction callback = null)
        {
            _tween = transform.DOLocalMove(position, _duration);

            await _tween.WithCancellation(token);
            
            callback?.Invoke();
        }
        
        private void OnDestroy()
        {
            _tween?.Kill();
        }
    }
}