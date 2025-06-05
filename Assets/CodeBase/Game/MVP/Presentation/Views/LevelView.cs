using System.Collections.Generic;
using Core.MVP.Views;
using Game.Elements;
using UnityEngine;
using Zenject;

namespace Game.MVP.Presentation.Views
{
    public class LevelView : ViewBase
    {
        public class Factory : PlaceholderFactory<LevelView> { }
        
        [field: SerializeField] public List<MagazineElement> MagazineElements { get; private set; }
        [field: SerializeField] public GameObject BlockObject { get; private set; }
    }
}