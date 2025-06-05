using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Animations;
using UnityEngine;
using Zenject;

namespace Game.Elements
{
    public class StoneElement : MonoBehaviour
    {
        public class Pool : MonoMemoryPool<StoneElement>
        {
            protected override void OnDespawned(StoneElement item)
            {
                base.OnDespawned(item);
                item.SetDefault();
            }
        }

        [SerializeField] private MoveAnimation _animation;

        private CancellationTokenSource _cts;

        public void Init(Transform parent)
        {
            transform.SetParent(parent, false);
            gameObject.transform.localPosition = Vector3.zero;
        }

        public async UniTask MoveAsync(Transform parent, Vector3 position)
        {
            try
            {
                transform.SetParent(parent, false);
                gameObject.transform.localScale = Vector3.one;
                _cts = new CancellationTokenSource();
                await _animation.DoMoveAnimationAsync(position, _cts.Token);
            }
            catch (OperationCanceledException e)
            {
                transform.localPosition = position;
                Debug.Log($"Operation Cancelled:\n{e}");
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
            }
        }

        private void OnDestroy()
        {
            _cts?.Dispose();
            _cts = null;
        }

        private void SetDefault()
        {
            transform.localPosition = Vector3.zero;
        }
    }
}