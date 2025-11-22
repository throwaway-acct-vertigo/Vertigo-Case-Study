using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Screens")] [SerializeField] private GameObject ui_screen_mainmenu;
    [SerializeField] private GameObject ui_screen_gameplay;
    [SerializeField] private GameObject ui_screen_result;

    [Header("Buttons - Auto-assigned")] private List<UIButton> ui_button_spin;
    private UIButton ui_button_leave;
    private UIButton ui_button_mainMenu;

    private CanvasGroup currentScreenCanvasGroup;
    [SerializeField] private Image _currentZoneBackgroundImage;
    [SerializeField] private List<RoundIndicator> _roundIndicators = new List<RoundIndicator>();
    private bool _rebuildOnLevelEnd = false;
    [SerializeField] private List<RewardSlotController> _rewardSlotControllers;
    [SerializeField] private RewardCardSlotController _rewardCardSlotController;
    private Vector2? _defaultAnchoredPosition;

    public void Initialize()
    {
        SubscribeToEvents();
        _rebuildOnLevelEnd = false;
    }

    private void OnValidate()
    {
        if (ui_screen_gameplay != null)
        {
            UIButton[] gameplayButtons = ui_screen_gameplay.GetComponentsInChildren<UIButton>(true);
            ui_button_spin = new List<UIButton>();
            foreach (var btn in gameplayButtons)
            {
                if (btn.name.Contains("Spin"))
                {
                    ui_button_spin.Add(btn);
                }
                else if (btn.name.Contains("leave"))
                {
                    ui_button_leave = btn;
                }
                else if (btn.name.Contains("mainMenu"))
                {
                    ui_button_mainMenu = btn;
                }
            }
        }
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        GameEvents.OnZoneChanged += UpdateZoneDisplay;
        GameEvents.OnZoneChanged += OnLevelPass;
        GameEvents.OnRewardCollected += OnRewardCollected;
        GameEvents.OnBombHit += OnBombHit;
        GameEvents.OnPlayerLeave += OnPlayerLeave;
        GameEvents.OnGameStateChanged += OnGameStateChanged;
    }

    private void UnsubscribeFromEvents()
    {
        GameEvents.OnZoneChanged -= UpdateZoneDisplay;
        GameEvents.OnZoneChanged -= OnLevelPass;
        GameEvents.OnRewardCollected -= OnRewardCollected;
        GameEvents.OnBombHit -= OnBombHit;
        GameEvents.OnPlayerLeave -= OnPlayerLeave;
        GameEvents.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnRewardCollected(Reward reward)
    {
        Vector3 initialPosition = _rewardCardSlotController.transform.position;
        _rewardCardSlotController.ResetContainer(reward);
        _rewardCardSlotController.gameObject.SetActive(true);
        _rewardCardSlotController.transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.InOutBack).OnComplete(() =>
            {
                _rewardCardSlotController.interactable = true;
                float localY = _rewardCardSlotController.targetGraphic.transform.localPosition.y;
                Tween tween = _rewardCardSlotController.targetGraphic.transform.DOLocalMoveY(localY + 5, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                _rewardCardSlotController.OnSelected = () =>
                {
                    tween.Kill(true);
                    _rewardCardSlotController.targetGraphic.transform.localPosition = Vector3.zero;
                    _rewardCardSlotController.transform.DOMove(_rewardSlotControllers.Find(a => a.Container.ID == reward.ID).transform.position, 0.5f).SetEase(Ease.OutQuint);
                    _rewardCardSlotController.transform.DOScale(0.3f * Vector3.one, 0.5f).SetEase(Ease.OutSine)
                        .OnComplete(() =>
                        {
                            _rewardCardSlotController.gameObject.SetActive(false);
                            _rewardCardSlotController.transform.position = initialPosition;
                            _rewardCardSlotController.transform.localScale = 0.3f * Vector3.one;
                            GameEvents.RewardAnimationFinished?.Invoke();
                        });
                };
            });
    }

    public void ShowGameplayScreen()
    {
        HideAllScreens();
        ShowScreen(ui_screen_gameplay);
        UpdateButtonStates();
    }

    public void ShowMainMenuScreen()
    {
        HideAllScreens();
        ShowScreen(ui_screen_mainmenu);
        UpdateButtonStates();
    }

    private void OnBombHit() => RepositionUiObjects();
    private void OnPlayerLeave(List<Reward> _) => RepositionUiObjects();

    private void RepositionUiObjects()
    {
        _rebuildOnLevelEnd = false;
        foreach (RewardSlotController rewardSlotController in _rewardSlotControllers)
        {
            rewardSlotController.ResetContainer();
        }

        foreach (RoundIndicator roundIndicator in _roundIndicators)
        {
            roundIndicator.ResetText();
        }

        RectTransform parentRect = (RectTransform)_roundIndicators[0].transform.parent;
        parentRect.anchoredPosition = _defaultAnchoredPosition.Value;
    }

    public void ShowResultScreen(List<Reward> reward)
    {
        HideAllScreens();
        if (reward != null)
        {
            ui_screen_result.GetComponent<ResultScreen>().ShowGoodResults(reward);
        }
        else
        {
            ui_screen_result.GetComponent<ResultScreen>().ShowBadResults();
        }

        ShowScreen(ui_screen_result);
    }

    private void OnLevelPass(int zone)
    {
        RectTransform parentRect = (RectTransform)_roundIndicators[0].transform.parent;
        if (zone == 1)
        {
            if (_defaultAnchoredPosition.HasValue)
            {
                RepositionUiObjects();
            }
            else
            {
                _defaultAnchoredPosition = parentRect.anchoredPosition;
            }

            return;
        }

        transform.DOComplete();
        float width = ((RectTransform)_roundIndicators[0].transform).rect.width;
        Vector2 currentPos = parentRect.anchoredPosition;
        parentRect.DOAnchorPos(new Vector2(currentPos.x - width, currentPos.y), .5f)
            .onComplete += RebuildTopOnLevelPass;
    }

    private void RebuildTopOnLevelPass()
    {
        if (_rebuildOnLevelEnd == false)
        {
            return;
        }

        int newMax = int.Parse(_roundIndicators[^1].Text) + 1;

        _roundIndicators[^1].Text = newMax.ToString();
        for (int i = _roundIndicators.Count - 2; i >= 0; i--)
        {
            _roundIndicators[i].Text = $"{--newMax}";
        }

        float width = ((RectTransform)_roundIndicators[0].transform).rect.width;
        RectTransform parentRect = (RectTransform)_roundIndicators[0].transform.parent;

        parentRect.anchoredPosition += width * Vector2.right;
    }

    private void ShowScreen(GameObject screen)
    {
        if (screen != null)
        {
            screen.SetActive(true);

            CanvasGroup cg = screen.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = screen.AddComponent<CanvasGroup>();
            }

            cg.alpha = 0f;
            cg.DOFade(1f, 0.3f);
        }
    }

    private void HideAllScreens()
    {
        if (ui_screen_mainmenu != null)
        {
            ui_screen_mainmenu.SetActive(false);
        }

        if (ui_screen_gameplay != null)
        {
            ui_screen_gameplay.SetActive(false);
        }

        if (ui_screen_result != null)
        {
            ui_screen_result.SetActive(false);
        }
    }

    public void UpdateZoneDisplay(int zone)
    {
        ZoneType type = ZoneManager.Instance.GetCurrentZoneType();
        switch (type)
        {
            case ZoneType.Regular:
                _currentZoneBackgroundImage.color = Color.white;
                break;
            case ZoneType.Safe:
                _currentZoneBackgroundImage.color = Color.green;
                break;
            case ZoneType.Super:
                _currentZoneBackgroundImage.color = Color.yellow;
                break;
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
        ui_button_spin.ForEach(a => a.SetInteractable(false));
        ui_button_leave.SetInteractable(false);
        ui_button_mainMenu.SetInteractable(false);

        switch (newState)
        {
            case GameState.MainMenu:
                break;
            case GameState.Playing:
                ui_button_leave.SetInteractable(ZoneManager.Instance.IsCurrentZoneSafe());
                ui_button_spin.ForEach(a => a.SetInteractable(true));
                ui_button_mainMenu.SetInteractable(true);
                break;
            case GameState.Spinning:
                break;
            case GameState.CollectingReward:
                break;
            case GameState.GameOver:
                break;
        }
    }


    private void UpdateButtonStates()
    {
        if (GameManager.Instance != null)
        {
            if (ui_button_spin != null)
            {
                ui_button_spin.ForEach(a => a.SetInteractable(GameManager.Instance.CanSpin()));
            }
        }
    }

    public void StartRebuildNumberIndicators()
    {
        _rebuildOnLevelEnd = true;
    }

    public void UpdateRewards(List<Reward> rewards)
    {
        foreach (RewardSlotController rewardSlotController in _rewardSlotControllers)
        {
            rewardSlotController.ResetContainer();
        }

        for (int i = 0; i < rewards.Count; i++)
        {
            _rewardSlotControllers[i].ResetContainer(rewards[i]);
        }
    }
}