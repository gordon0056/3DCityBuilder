using System;
using Core.MVP.Views;
using UnityEngine;

namespace Game.MVP.Presentation.Views
{
    public class PlayerView : ViewBase
    {
        [field: SerializeField] public DynamicJoystick Joystick { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Transform ElementsPlace { get; private set; }

        public event Action<Collider> TriggerEnter;
        public event Action<Collider> TriggerStay;
        public event Action<Collider> TriggerExit;

        private void OnTriggerEnter(Collider collider)
        {
            TriggerEnter?.Invoke(collider);
        }

        private void OnTriggerStay(Collider collider)
        {
            TriggerStay?.Invoke(collider);
        }
        
        private void OnTriggerExit(Collider collider)
        {
            TriggerExit?.Invoke(collider);
        }
    }
}