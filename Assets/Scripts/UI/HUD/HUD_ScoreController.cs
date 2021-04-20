using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//State Manager
public partial class HUD_ScoreController
{
    protected override void OnPrepare()
    {
        ShowView();
    }

    protected override void OnMainMenu()
    {
        HideView();
    }
}

public partial class HUD_ScoreController : HUDController
{
    [Header("Dependencies")] 
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private PopupManager popupManager;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI highScoreLabel;
    [SerializeField] private TextMeshProUGUI scoreLabel;

    [SerializeField] private Button pauseBtn;

    [SerializeField] private ScoreLabelAnimator scoreLabelAnimator;

    [SerializeField] private GameObject highScoreGO;
    private bool isHighScoreGOActive;

    protected override void Init()
    {
        base.Init();
        
        pauseBtn.onClick.AddListener(OnClickPauseBtn);

        scoreLabelAnimator.SetInstances(scoreLabel);

        OnUpdateScore(scoreManager.CurrentScore);
        OnUpdateHighScore(scoreManager.HighScore);

        if (scoreManager.HighScore <= 0)
        {
            isHighScoreGOActive = false;
            highScoreGO.SetActive(false);
        }
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        scoreManager.UpdateScore += OnUpdateScore;
        scoreManager.UpdateHighScore += OnUpdateHighScore;
        scoreLabelAnimator.UpdateAnimatedScore += UpdateScoreLabel;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        scoreManager.UpdateScore -= OnUpdateScore;
        scoreManager.UpdateHighScore -= OnUpdateHighScore;
        scoreLabelAnimator.UpdateAnimatedScore -= UpdateScoreLabel;
    }

    void Update()
    {
        scoreLabelAnimator.OnUpdate();
    }
    
    private void OnClickPauseBtn()
    {
        popupManager.ShowUI<PausePopupController>();
    }

    private void OnUpdateScore(int newCurrentScore)
    {
        scoreLabelAnimator.OnUpdateScore(newCurrentScore);
    }
    
    private void OnUpdateHighScore(int newHighScore)
    {
        if (isHighScoreGOActive == false && scoreManager.HighScore > 0)
        {
            isHighScoreGOActive = true;
            highScoreGO.SetActive(true);
        }
        
        highScoreLabel.SetTextFormat("{0}", scoreManager.HighScore);
    }

    private void UpdateScoreLabel(int newCurrentScore)
    {
        scoreLabel.SetTextFormat("{0}", newCurrentScore);
    }
}
