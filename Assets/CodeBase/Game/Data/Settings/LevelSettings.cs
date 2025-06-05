using UnityEngine;

namespace Game.Data.Settings
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Settings/Settings/Create Level Settings", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        [field: SerializeField] public float LevelsOffsetX { get; private set; }
        [field: SerializeField] public float LevelsCount { get; private set; }
    }
}