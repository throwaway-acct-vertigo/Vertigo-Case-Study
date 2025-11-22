using UnityEngine;

namespace DefaultNamespace
{
    public class RegularZoneStrategy : IZoneStrategy
    {
        public bool HasBomb() => true;
        public bool CanLeave() => false;
        public ZoneType GetZoneType() => ZoneType.Regular;
        public float CurrentMultiplier => Mathf.Pow(1.05f, ZoneManager.Instance.CurrentZone);
    }
}