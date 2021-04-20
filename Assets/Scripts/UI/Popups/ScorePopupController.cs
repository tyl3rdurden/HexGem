using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class ScorePopupController
{
    protected override void OnFinish()
    {
        ShowView();
    }
}

public partial class ScorePopupController : PopupController
{
    [Header("Dependencies")]
    [SerializeField] private ScoreManager scoreManager;
    
    [Header("UI")] 
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TextMeshProUGUI comboLabel;
    [SerializeField] private TextMeshProUGUI hiScoreLabel;

    [SerializeField] private Button restartBtn;
    [SerializeField] private Button quitBtn;
    
    [SerializeField] private GameObject newHiScoreNotification;
    
    protected override void Init()
    {
        base.Init();

        scoreManager.UpdateHighScore += OnNewHighScore;
        
        restartBtn.onClick.AddListener(OnClickRestartBtn);
        quitBtn.onClick.AddListener(OnClickQuitBtn);

        newHiScoreNotification.SetActive(false);
    }

    private void OnNewHighScore(int newHighScore)
    {
        newHiScoreNotification.SetActive(true);
    }

    protected override void OnShow()
    {
        base.OnShow();
        SetScoreLabels();
    }

    protected override void AfterMoveOut()
    {
        base.AfterMoveOut();
        newHiScoreNotification.SetActive(false);
    }

    private void SetScoreLabels()
    {
        scoreLabel.text = scoreManager.CurrentScore.ToString();
        hiScoreLabel.text = scoreManager.HighScore.ToString();
    }
    
    private void OnClickRestartBtn()
    {
        HideView();
        gameStateManager.ChangeState(GameStateType.Prepare);
    }

    private void OnClickQuitBtn()
    {
        popupManager.ShowUI<QuitPopupController>();
    }
}
