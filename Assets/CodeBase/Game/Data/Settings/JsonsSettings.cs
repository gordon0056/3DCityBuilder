using UnityEngine;

namespace Game.Data.Settings
{
    [CreateAssetMenu(fileName = "JsonsSettings", menuName = "Settings/Settings/Create Json Settings", order = 0)]
    public class JsonsSettings : ScriptableObject
    {
        [field: SerializeField] public string SaveJsonName { get; private set; }
        [field: SerializeField] public float LoadDuration { get; private set; }
    }
}