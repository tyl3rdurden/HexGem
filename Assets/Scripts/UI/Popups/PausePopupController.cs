using UnityEngine;
using UnityEngine.UI;

public class PausePopupController : PopupController
{
    [Header("UI")]
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button quitBtn;

    protected override void Init()
    {
        base.Init();
        
        restartBtn.onClick.AddListener(OnClickRestartBtn);
        optionBtn.onClick.AddListener(OnClickOptionBtn);
        quitBtn.onClick.AddListener(OnClickQuitBtn);
    }

    private void OnClickRestartBtn()
    {
        HideView();
        gameStateManager.ChangeState(GameStateType.Prepare);
    }
    
    private void OnClickOptionBtn()
    {
        popupManager.ShowUI<OptionPopupController>();
    }

    private void OnClickQuitBtn()
    {
        HideView();
    }
}
