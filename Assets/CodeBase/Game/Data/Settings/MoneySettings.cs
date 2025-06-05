using UnityEngine;

namespace Game.Data.Settings
{
    [CreateAssetMenu(fileName = "MoneySettings", menuName = "Settings/Settings/Create Money Settings", order = 0)]
    public class MoneySettings : ScriptableObject
    {
        [field: SerializeField] public int BuildingPriceMultiplier { get; private set; }
        [field: SerializeField] public int MoneyTickMultiplier { get; private set; }
        [field: SerializeField] public float TickSeconds { get; private set; }
        [field: SerializeField] public int StartMoney { get; private set; }
    }
}