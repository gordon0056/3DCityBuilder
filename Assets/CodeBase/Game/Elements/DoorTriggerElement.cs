using Game.Data.Enums;
using UnityEngine;

namespace Game.Elements
{
    public class DoorTriggerElement : MonoBehaviour
    {
        [field: SerializeField] public MagazineType Type { get; private set; }
    }
}