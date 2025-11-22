using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class ZoneManager : Singleton<ZoneManager>
    {
        private List<WheelConfigSO> _regularZoneWheelConfigs;
        private List<WheelConfigSO> _safeZoneWheelConfigs;
        private List<WheelConfigSO> _superZoneWheelConfigs;
        public int CurrentZone { get; private set; }

        public IZoneStrategy CurrentStrategy;

        protected override void Awake()
        {
            base.Awake();
            LoadWheelConfigs(ZoneType.Regular);
            LoadWheelConfigs(ZoneType.Safe);
            LoadWheelConfigs(ZoneType.Super);
        }

        public void Initialize()
        {
            CurrentZone = 1;
            UpdateZoneStrategy();
        }

        public void ResetToZone(int zone)
        {
            CurrentZone = zone;
            UpdateZoneStrategy();
            GameEvents.OnZoneChanged?.Invoke(CurrentZone);
        }

        public void AdvanceZone()
        {
            CurrentZone++;
            UpdateZoneStrategy();
            GameEvents.OnZoneChanged?.Invoke(CurrentZone);
        }

        private void UpdateZoneStrategy()
        {
            CurrentStrategy = GetCurrentZoneType() switch
            {
                ZoneType.Regular => new RegularZoneStrategy(),
                ZoneType.Safe => new SafeZoneStrategy(),
                ZoneType.Super => new SuperZoneStrategy(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public WheelConfigSO GetCurrentWheelConfig()
        {
            return GetConfig(GetCurrentZoneType());
        }

        private WheelConfigSO GetConfig(ZoneType zoneType)
        {
            List<WheelConfigSO> list;
            WheelConfigSO config;
            switch (zoneType)
            {
                case ZoneType.Regular:
                    config = _regularZoneWheelConfigs[Random.Range(0, _regularZoneWheelConfigs.Count)];
                    list = _regularZoneWheelConfigs;
                    break;
                case ZoneType.Safe:
                    config = _safeZoneWheelConfigs[Random.Range(0, _regularZoneWheelConfigs.Count)];
                    list = _safeZoneWheelConfigs;
                    break;
                case ZoneType.Super:
                    config = _superZoneWheelConfigs[Random.Range(0, _regularZoneWheelConfigs.Count)];
                    list = _superZoneWheelConfigs;
                    break;
                default:
                    return null;
            }

            list.Remove(config);
            if (list.Count == 0)
            {
                LoadWheelConfigs(zoneType);
            }

            return config;
        }

        private void LoadWheelConfigs(ZoneType zoneType)
        {
            // Pick the address
            string address = zoneType switch
            {
                ZoneType.Regular => "regularZoneConfig",
                ZoneType.Safe => "safeZoneConfig",
                ZoneType.Super => "superZoneConfig",
                _ => null
            };

            // Pick the field setter (cleanest way to avoid switches inside the callback)
            Action<List<WheelConfigSO>> assign = zoneType switch
            {
                ZoneType.Regular => list => _regularZoneWheelConfigs = list,
                ZoneType.Safe => list => _safeZoneWheelConfigs = list,
                ZoneType.Super => list => _superZoneWheelConfigs = list,
                _ => null
            };

            var handle = ResourceManager.LoadAssets<WheelConfigSO>(address);

            handle.Completed += h => { assign?.Invoke(new List<WheelConfigSO>(h.Result)); };
        }

        public bool IsCurrentZoneSafe()
        {
            return CurrentZone % 5 == 0 || CurrentZone % 30 == 0;
        }

        public ZoneType GetCurrentZoneType()
        {
            if (CurrentZone % 30 == 0)
            {
                return ZoneType.Super;
            }

            if (CurrentZone % 5 == 0)
            {
                return ZoneType.Safe;
            }

            return ZoneType.Regular;
        }
    }
}