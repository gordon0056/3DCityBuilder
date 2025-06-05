using UnityEngine;

namespace Game.Elements
{
    public class MoneyShowElement : MonoBehaviour
    {
        [field: SerializeField] public Transform LeftPlace { get; private set; }
        [field: SerializeField] public Transform RightPlace { get; private set; }
    }
}