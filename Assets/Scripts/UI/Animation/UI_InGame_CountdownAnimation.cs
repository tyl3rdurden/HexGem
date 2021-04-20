using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class UI_InGame_CountdownAnimation : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private GameStateManager gameStateManager;
    
    [ChildGameObjectsOnly]
    [SerializeField] private TextMeshProUGUI[] countdownLabels;
    [SerializeField] private DOTweenPlayerSO[] countdownLabelAnims;

    [BoxGroup("Debug")] 
    [SerializeField] private bool skipCountdown;

    [SerializeField]
    private float startY = -400f;

    void Awake()
    {
        TurnOffCanvas();
        
        InitCountDownLabelAnimations();
        gameStateManager.GameStateDic[GameStateType.Countdown].OnPreEnterState += OnCountdown;
    }

    private void InitCountDownLabelAnimations()
    {
        for (int i = 0; i < countdownLabelAnims.Length; i++)
        {
            countdownLabelAnims[i].SetTargetTransform((RectTransform)countdownLabels[i].transform);
            countdownLabelAnims[i].SetDelay(i);
            
            if (i == countdownLabels.Length - 2) // Label 1
                countdownLabelAnims[i].OnFinish += MoveToNextState;
            
            if (i == countdownLabels.Length - 1) // Label GO
                countdownLabelAnims[i].OnFinish += TurnOffCanvas;
        }
    }

    private void OnCountdown()
    {
#if UNITY_EDITOR
        if (IsCountdownSkipped())
            return;
#endif
        
        gameObject.SetActive(true);
        
        for (int index = 0; index < countdownLabels.Length; index++)
        {
            TextMeshProUGUI countdownLabel = countdownLabels[index];
            
            Transform trans = countdownLabel.transform;
            trans.localPosition = trans.localPosition.WithY(startY);
        }

        StartCountdownAnimation();
    }

    private void StartCountdownAnimation()
    {
        for (int i = 0; i < countdownLabels.Length; i++)
        {
            TextMeshProUGUI countdownLabel = countdownLabels[i];
            Color zeroAlpha = countdownLabel.color;
            zeroAlpha.a = 0;
            countdownLabel.color = zeroAlpha;
            
            countdownLabelAnims[i].Play();
        }
    }

    private void MoveToNextState()
    {
        gameStateManager.ChangeState(GameStateType.InGame);
    }

    private void TurnOffCanvas()
    {
        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    private bool IsCountdownSkipped()
    {
        if (skipCountdown)
        {
            MoveToNextState();
        }

        return skipCountdown;
    }
#endif
}
