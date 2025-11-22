namespace DefaultNamespace
{
    public interface IZoneStrategy
    {
        bool HasBomb();
        bool CanLeave();
        ZoneType GetZoneType();
        float CurrentMultiplier { get; }
    }
}