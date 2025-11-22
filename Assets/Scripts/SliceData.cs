using DefaultNamespace;
using UnityEngine;

[System.Serializable]
public class SliceData
{
    public string sliceName;
    public bool isBomb;
    public Reward Reward;
    public Sprite iconSprite;

    public SliceData(bool bomb, Reward reward)
    {
        isBomb = bomb;
        Reward = reward;
        if (bomb)
        {
            iconSprite = RewardManager.Instance.BombSprite;
            sliceName = "bomb";
        }
        else
        {
            iconSprite = reward.Icon;
            sliceName = reward.Name;
        }
    }

    public static SliceData Generate(float multiplier)
    {
        RewardData rewardData = RewardManager.Instance.RewardData[Random.Range(0, RewardManager.Instance.RewardData.Count)]; //todo: unique reward generation
        Reward reward = RewardGenerator.GenerateReward(rewardData, (int)(rewardData.DefaultQuantity * multiplier));
        return new SliceData(false, reward);
    }
}