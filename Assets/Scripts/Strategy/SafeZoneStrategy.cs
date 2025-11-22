using UnityEngine;

namespace DefaultNamespace
{
    internal class SafeZoneStrategy : IZoneStrategy
    {
        public bool HasBomb() => false;
        public bool CanLeave() => true;
        public ZoneType GetZoneType() => ZoneType.Safe;
        public float CurrentMultiplier => Mathf.Pow(3f, ZoneManager.Instance.CurrentZone / 5f);
    }
}