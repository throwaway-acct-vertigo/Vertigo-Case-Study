using UnityEngine;
using System.Collections.Generic;
using DefaultNamespace;

[CreateAssetMenu(fileName = "WheelConfig", menuName = "Game/Wheel Configuration")]
public class WheelConfigSO : ScriptableObject
{
    [Header("Wheel Info")] public string wheelName;
    public ZoneType zoneType;

    [Header("Slices")] public List<SliceData> slices = new List<SliceData>(new SliceData[8]);

    [Header("Settings")] public bool hasBomb = true;
    public int rewardMultiplier = 1;
    
    public void GenerateSlices(float runtimeMultiplier = 1f)
    {
        if (name.Contains("Regular"))
        {
            zoneType = ZoneType.Regular;
            hasBomb = true;
        }
        else if (name.Contains("Safe"))
        {
            zoneType = ZoneType.Safe;
            hasBomb = false;
        }
        else
        {
            zoneType = ZoneType.Super;
            hasBomb = false;
        }

        GenerateSlicesWithBomb(runtimeMultiplier);
        if (hasBomb == false)
        {
            slices[7] = SliceData.Generate(rewardMultiplier * runtimeMultiplier);
        }
    }

    private void GenerateSlicesWithBomb(float runtimeMultiplier = 1f)
    {
        for (int i = 0; i < 7; i++)
        {
            slices[i] = SliceData.Generate(rewardMultiplier * runtimeMultiplier);
        }

        slices[7] = new SliceData(true, null);
    }
}