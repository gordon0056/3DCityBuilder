using UnityEngine;
using Zenject;

namespace Game.Elements
{
    public class StoneGettingElement : MonoBehaviour
    {
        private StoneElement.Pool _pool;
        
        [Inject]
        public void Construct(StoneElement.Pool pool)
        {
            _pool = pool;
        }

        public StoneElement GetStone()
        {
            StoneElement stone = _pool.Spawn();
            stone.Init(transform);
            return stone;
        }
    }
}