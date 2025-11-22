public static class RewardGenerator
{
    public static Reward GenerateReward(RewardData reward, int quantity)
    {
        return new Reward()
        {
            ID = reward.ID,
            Amount = quantity,
            Name = reward.Name,
            Icon = reward.Icon,
            Type = reward.RewardType,
        };
    }
}