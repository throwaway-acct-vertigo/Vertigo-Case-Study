using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Managers")] [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private WheelController wheelController;
    [SerializeField] private RewardManager rewardManager;
    [SerializeField] private UIManager uiManager;

    private GameState _currentState;

    protected override void Awake()
    {
        base.Awake();
        InitializeGame();
    }

    private void OnEnable()
    {
        GameEvents.OnWheelSpinComplete += HandleWheelSpinComplete;
        GameEvents.OnBombHit += HandleBombHit;
        GameEvents.OnPlayerLeave += HandlePlayerLeave;
        GameEvents.RewardAnimationFinished += OnRewardCollectionEnded;
    }

    private void OnDisable()
    {
        GameEvents.OnWheelSpinComplete -= HandleWheelSpinComplete;
        GameEvents.OnBombHit -= HandleBombHit;
        GameEvents.OnPlayerLeave -= HandlePlayerLeave;
        GameEvents.RewardAnimationFinished -= OnRewardCollectionEnded;
    }

    private void InitializeGame()
    {
        SetCurrentState(GameState.MainMenu);
        zoneManager.Initialize();
        rewardManager.Initialize();
        uiManager.Initialize();
    }

    public void StartGame()
    {
        SetCurrentState(GameState.Playing);
        zoneManager.ResetToZone(1);
        rewardManager.ClearRewards();
        uiManager.ShowGameplayScreen();
        PrepareWheel();
    }

    private void SetCurrentState(GameState newState)
    {
        _currentState = newState;
        GameEvents.OnGameStateChanged?.Invoke(newState);
    }

    public void SpinWheel()
    {
        if (_currentState != GameState.Playing)
        {
            return;
        }

        SetCurrentState(GameState.Spinning);
        wheelController.Spin();
    }

    private void LeaveGame(List<Reward> rewards)
    {
        SetCurrentState(GameState.GameOver);
        uiManager.ShowResultScreen(rewards);
    }

    private void PrepareWheel()
    {
        WheelConfigSO config = zoneManager.GetCurrentWheelConfig();
        wheelController.SetupWheel(config);
        uiManager.UpdateZoneDisplay(zoneManager.CurrentZone);
    }

    private void HandleWheelSpinComplete(int sliceIndex)
    {
        SliceData selectedSlice = wheelController.GetSliceData(sliceIndex);

        if (selectedSlice.isBomb)
        {
            GameEvents.OnBombHit?.Invoke();
        }
        else
        {
            rewardManager.AddReward(selectedSlice.Reward);
            GameEvents.OnRewardCollected?.Invoke(selectedSlice.Reward);

            zoneManager.AdvanceZone();
            PrepareWheel();
            SetCurrentState(GameState.CollectingReward);
        }
    }

    private void OnRewardCollectionEnded()
    {
        SetCurrentState(GameState.Playing);
    }

    private void HandleBombHit()
    {
        SetCurrentState(GameState.GameOver);
        rewardManager.ClearRewards();
        uiManager.ShowResultScreen(null);
    }

    private void HandlePlayerLeave(List<Reward> rewards)
    {
        LeaveGame(rewards);
    }

    private bool CanLeave()
    {
        return _currentState == GameState.Playing && zoneManager.IsCurrentZoneSafe();
    }

    public bool CanSpin()
    {
        return _currentState == GameState.Playing;
    }
}