namespace Game.Data.Dto
{
    public struct MagazineProgressDto
    {
        public readonly int Current;
        public readonly int Max;
        public readonly bool IsActive;

        public MagazineProgressDto(int current, int max, bool isActive)
        {
            Current = current;
            Max = max;
            IsActive = isActive;
        }
    }
}