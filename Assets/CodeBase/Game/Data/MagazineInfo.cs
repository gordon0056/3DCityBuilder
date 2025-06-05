using System;
using Game.Data.Enums;

namespace Game.Data
{
    [Serializable]
    public class MagazineInfo
    {
        public MagazineType Type;
        public bool IsBought;
        public int CompletedBuilds;
        public int CurrentBuildParts;

        public MagazineInfo()
        {
            
        }

        public MagazineInfo(MagazineType type)
        {
            Type = type;
        }
    }
}