using UnityEngine;

namespace DefaultNamespace
{
    public class SuperZoneStrategy : IZoneStrategy
    {
        public bool HasBomb() => false;
        public bool CanLeave() => true;
        public ZoneType GetZoneType() => ZoneType.Super;
        public float CurrentMultiplier => Mathf.Pow(10, ZoneManager.Instance.CurrentZone / 30f);
    }
}