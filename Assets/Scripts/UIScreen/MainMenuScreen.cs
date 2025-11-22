public class MainMenuScreen : UIScreen
{
    private UIButton ui_button_play;

    protected override void Awake()
    {
        base.Awake();
        SetupButtons();
    }

    private void OnValidate()
    {
        // Auto-find buttons
        UIButton[] buttons = GetComponentsInChildren<UIButton>(true);
        foreach (var btn in buttons)
        {
            if (btn.name.Contains("StartGame"))
            {
                ui_button_play = btn;
            }
        }
    }

    private void SetupButtons()
    {
        if (ui_button_play != null)
        {
            ui_button_play.AddClickListener(OnPlayClicked);
        }
    }

    private void OnPlayClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }
}