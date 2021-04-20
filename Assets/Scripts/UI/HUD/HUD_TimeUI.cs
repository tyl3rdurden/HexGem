using Cysharp.Text;
using TMPro;
using UnityEngine;

public partial class HUD_TimeUI
{
    protected override void OnPrepare()
    {
        ShowView();
        ResetTime();
    }

    protected override void OnMainMenu()
    {
        HideView();
    }
}

public partial class HUD_TimeUI : HUDController 
{
    [Header("Dependencies")]
    [SerializeField] private TimeManager timeManager;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timeLabel;
    [SerializeField] private CompositeFillSprite timeBar;

    [SerializeField] private DOTweenPlayerSO timeLabelAnimation;
    
    private TimeMode timeMode;

    private int prevTime = -1;

    //if reset game during animation, text starts animation again from animated upward position
    //if not reset, text can continue moving upward during multiple plays
    private Vector2 timeLabelOriginalPosition;

    protected override void Init()
    {
        base.Init();
        timeLabelAnimation.SetTargetTransform((RectTransform)timeLabel.transform);

        timeMode = timeManager.CurrentTimeMode;

        timeLabelOriginalPosition = timeLabel.rectTransform.anchoredPosition;
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        timeManager.OnTimeChange += OnTimeChange;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        timeManager.OnTimeChange -= OnTimeChange;
    }

    protected override void OnShow()
    {
        base.OnShow();
        timeMode = timeManager.CurrentTimeMode;
    }

    private void OnTimeChange(float currentTime)
    {
        PlayTimeLabelAnimation();
        SetTimeBar();
        
        void PlayTimeLabelAnimation()
        {
            if (prevTime == timeMode.CurrentTimeInt || timeMode.CurrentTimeInt == timeMode.DefaultTime) //game started and time passing but 1 second has not passed yet
                return;
            if (prevTime == -1)
                prevTime = timeMode.CurrentTimeInt;

            prevTime = timeMode.CurrentTimeInt;
            
            timeLabel.SetTextFormat("{0}", timeMode.CurrentTimeInt);
            timeLabelAnimation.Play();
        }
        
        void SetTimeBar()
        {
            timeBar.SetValue(timeMode.CurrentTime / timeMode.DefaultTime);
        }
    }

    private void ResetTime()
    {
        timeLabel.rectTransform.anchoredPosition = timeLabelOriginalPosition;
        timeLabel.SetTextFormat("{0}", timeMode.DefaultTime);
        timeBar.SetValue(1);
    }
}