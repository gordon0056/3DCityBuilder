using UnityEngine;

namespace Game.Data.Dto
{
    public class StonesProgressDto
    {
        public readonly int NeedStones;
        public readonly int MaxStones;
        public readonly Transform Target;

        public StonesProgressDto(int needStones, int maxStone, Transform target)
        {
            NeedStones = needStones;
            MaxStones = maxStone;
            Target = target;
        }
    }
}