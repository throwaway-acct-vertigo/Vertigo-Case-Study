using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : UIScreen
{
    private UIButton ui_button_restart;
    private UIButton ui_button_continue;
    private RewardSlotController[] _rewardSlotControllers;
    private GameObject _bombImage;
    private GameObject _rewards;
    private TMP_Text _primaryText;
    private TMP_Text _secondaryText;

    protected override void Awake()
    {
        base.Awake();
        SetupButtons();
    }
    
    private void OnValidate()
    {
        UIButton[] buttons = GetComponentsInChildren<UIButton>(true);
        _rewardSlotControllers = GetComponentsInChildren<RewardSlotController>(true);
        foreach (var btn in buttons)
        {
            if (btn.name.Contains("restart"))
            {
                ui_button_restart = btn;
            }
        }

        Image[] images = GetComponentsInChildren<Image>(true);
        foreach (Image img in images)
        {
            if (img.gameObject.name == "ui_screen_result_graphic_bomb")
            {
                _bombImage = img.gameObject;
            }
        }

        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);
        foreach (TMP_Text text in texts)
        {
            if (text.gameObject.name.Contains("primary"))
            {
                _primaryText = text;
            }
            else if (text.gameObject.name.Contains("secondary"))
            {
                _secondaryText = text;
            }
        }

        _rewards = _rewardSlotControllers[0].transform.parent.gameObject;
    }

    public void ShowBadResults()
    {
        _bombImage.SetActive(true);
        _rewards.SetActive(false);
        _primaryText.text = "Oh no!";
        _secondaryText.text = "You have hit a bomb, all your rewards are gone!";
    }

    public void ShowGoodResults(List<Reward> rewards)
    {
        _bombImage.SetActive(false);
        _rewards.SetActive(true);
        _primaryText.text = "Oh yes!";
        _secondaryText.text = "You walked away with your rewards!";
        for (int index = 0; index < _rewardSlotControllers.Length; index++)
        {
            RewardSlotController rewardSlotController = _rewardSlotControllers[index];
            rewardSlotController.ResetContainer(index < rewards.Count ? rewards[index] : null);
        }
    }


    private void SetupButtons()
    {
        if (ui_button_restart != null)
        {
            ui_button_restart.AddClickListener(OnRestartClicked);
        }
    }

    private void OnRestartClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }
}