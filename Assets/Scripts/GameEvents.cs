using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public static class GameEvents
    {
        public static Action<int> OnZoneChanged;
        public static Action<Reward> OnRewardCollected;
        public static Action OnBombHit;
        public static Action<List<Reward>> OnPlayerLeave;
        public static Action<int> OnWheelSpinComplete;
        public static Action RewardAnimationFinished;
        public static Action<GameState> OnGameStateChanged;
    }
}