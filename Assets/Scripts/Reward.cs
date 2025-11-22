using System;
using DefaultNamespace;
using UnityEngine;

[Serializable]
public class Reward : ISlotContainer
{
    public int Amount;
    public string ID;
    public string Name;
    [field: SerializeField] public Sprite Icon { get; set; }
    public RewardType Type;
}