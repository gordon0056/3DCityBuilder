using UnityEngine;

namespace Game.Data.Settings
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/Settings/Create Player Settings", order = 0)]
    public class PlayerSettings : ScriptableObject
    {
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public int MaxElementsCount { get; private set; }
        [field: SerializeField] public float ElementsOffset { get; private set; }
        [field: SerializeField] public float Delay { get; private set; }
    }
}