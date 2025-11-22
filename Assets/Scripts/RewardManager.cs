using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DefaultNamespace
{
    public class RewardManager : Singleton<RewardManager>
    {
        private List<Reward> _collectedRewards = new();
        public readonly List<RewardData> RewardData = new List<RewardData>();
        public Sprite BombSprite;

        public void Initialize()
        {
            ClearRewards();
            LoadRewardData();
            LoadBombImage();
        }

        private void LoadRewardData()
        {
            AsyncOperationHandle<IList<RewardData>> handler = ResourceManager.LoadAssets<RewardData>("rewardData");
            handler.Completed += (h) =>
            {
                RewardData.Clear();
                RewardData.AddRange(handler.Result);
            };
        }

        private void LoadBombImage()
        {
            AsyncOperationHandle<IList<Sprite>> handler = ResourceManager.LoadAssets<Sprite>("bombImage");
            handler.Completed += (h) => { BombSprite = handler.Result[0]; };
        }

        public void AddReward(Reward newReward)
        {
            newReward.Amount = Mathf.FloorToInt(newReward.Amount * ZoneManager.Instance.CurrentStrategy.CurrentMultiplier);
            foreach (Reward reward in _collectedRewards)
            {
                if (reward.ID == newReward.ID)
                {
                    reward.Amount += newReward.Amount;
                    UIManager.Instance.UpdateRewards(_collectedRewards);
                    return;
                }
            }

            _collectedRewards.Add(newReward);
            UIManager.Instance.UpdateRewards(_collectedRewards);
        }

        public void ClearRewards()
        {
            _collectedRewards.Clear();
            UIManager.Instance.UpdateRewards(_collectedRewards);
        }
        
        public List<Reward> GetRewards() => _collectedRewards;
    }
}