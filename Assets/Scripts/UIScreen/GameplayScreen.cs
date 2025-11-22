using System.Collections.Generic;
using DefaultNamespace;

public class GameplayScreen : UIScreen
{
    private List<UIButton> ui_button_spin = new();
    private UIButton ui_button_leave;
    private UIButton ui_button_mainMenu;


    private void Start()
    {
        SetupButtons();
    }

    private void OnValidate()
    {
        ui_button_spin.Clear();
        UIButton[] buttons = GetComponentsInChildren<UIButton>(true);
        foreach (var btn in buttons)
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

    private void SetupButtons()
    {
        if (ui_button_spin != null)
        {
            ui_button_spin.ForEach(a => a.AddClickListener(OnSpinClicked));
        }

        if (ui_button_leave != null)
        {
            ui_button_leave.AddClickListener(OnLeaveClicked);
        }
        
        if (ui_button_mainMenu != null)
        {
            ui_button_mainMenu.AddClickListener(OnMainMenuClicked);
        }
    }

    private void OnSpinClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SpinWheel();
        }
    }

    private void OnLeaveClicked()
    {
        if (GameManager.Instance != null)
        {
            GameEvents.OnPlayerLeave?.Invoke(RewardManager.Instance.GetRewards());
        }
    }

    private void OnMainMenuClicked()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowMainMenuScreen();
        }
    }
}