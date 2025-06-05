using Core.MVP.Views;
using Game.Animations;
using UnityEngine;

namespace Game.MVP.Presentation.Views
{
    public class LoadingView : CanvasGroupView
    {
        [field: SerializeField] public RotateAnimation Animation { get; private set; }
    }
}