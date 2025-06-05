using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Game.Animations
{
    public class RotateAnimation : MonoBehaviour
    {
        private const float _zValue = 360f;
        
        [SerializeField] private float _duration;
        
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _tween;
        
        public void DoRotateAnimation()
        {
            // transform.DOLocalRotate(Vector3.forward * 360f, _duration, RotateMode.LocalAxisAdd);
            
            float z = transform.localRotation.z + _zValue;
            _tween = transform.DOLocalRotate(new Vector3(0f,0f, z), _duration, RotateMode.LocalAxisAdd);
            
            _tween.SetRecyclable(true).SetEase(Ease.Linear).OnComplete(() =>
            {
                z = transform.localRotation.z + _zValue;
                _tween.Restart();
            });
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }
    }
}