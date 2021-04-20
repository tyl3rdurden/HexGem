using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract partial class UIController : MonoBehaviour //Has Custom Editor
{
    [Header("Dependencies")] 
    [InlineProperty()]
    [SerializeField] protected GameStateManager gameStateManager;
    
    [Header("Animations")]
    [SerializeField] private DOTweenPlayerSO animIn;
    [SerializeField] private DOTweenPlayerSO animOut;

    [Header("UI")] 
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    private bool isGraphicRaycasterExist;

    private RectTransform cachedRectTransform;

    public bool IsActive { get; private set; }
    
    private void Awake()
    {
        Init();
        AddListeners();

        isGraphicRaycasterExist = graphicRaycaster != null;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }    
    
    //Called once during lifetime
    protected virtual void Init()
    {
        gameObject.SetActive(false); //Have GOs off by default

        SetTweens();

        animIn.OnFinish += AfterMoveIn;
        animOut.OnFinish += AfterMoveOut;

        animOut.SetToEndValue();
    }
    
    public virtual void ShowView()
    {
        gameObject.SetActive(true);
        IsActive = true;
        
        OnShow();
        MoveInAnimation();
    }

    public virtual void HideView()
    {
        OnHide();
        MoveOutAnimation();
    }
    
    protected virtual void AddListeners()
    {
        gameStateManager.OnChangeState += OnChangeGameState;
    }

    protected virtual void RemoveListeners()
    {
        gameStateManager.OnChangeState -= OnChangeGameState;
    }

    protected virtual void OnShow()
    {
        if (isGraphicRaycasterExist)
            graphicRaycaster.enabled = true;
    }
    
    protected virtual void OnHide() {}

    protected virtual void AfterMoveIn() {}

    protected virtual void AfterMoveOut()
    {
        gameObject.SetActive(false);
    }
    
    #region GameStateManagement
    
    private void OnChangeGameState(GameStateType gameStateType)
    {
        switch (gameStateType)
        {
            case GameStateType.MainMenu:
                OnMainMenu();
                break;
            case GameStateType.Load:
                OnLoad();
                break;
            case GameStateType.Prepare:
                OnPrepare();
                break;
            case GameStateType.Countdown:
                OnCountdown();
                break;
            case GameStateType.InGame:
                OnInGame();
                break;
            case GameStateType.Bonus:
                OnBonus();
                break;
            case GameStateType.Finish:
                OnFinish();
                break;
        }
    }

    protected virtual void OnMainMenu() { }
    
    protected virtual void OnLoad() { }
    
    protected virtual void OnPrepare() { }
    
    protected virtual void OnCountdown() { }
    
    protected virtual void OnInGame() { }
    
    protected virtual void OnBonus() { }
    
    protected virtual void OnFinish() { }
    
    #endregion

    private void MoveInAnimation()
    {
        animIn.Play();
    }
    
    private void MoveOutAnimation()
    {
        animOut.Play();
        IsActive = false;
        
        if (isGraphicRaycasterExist)
            graphicRaycaster.enabled = false;
    }

    private void SetTweens()
    {
        cachedRectTransform = (RectTransform)transform;
        
        animIn.SetTargetTransform(cachedRectTransform);
        animOut.SetTargetTransform(cachedRectTransform);
    }
}

#if UNITY_EDITOR
public abstract partial class UIController
{
    public void Editor_ShowView()
    {
        animIn.Editor_Init();
        animOut.Editor_Init();
        SetTweens();

        animOut.SetToEndValue();
    }
    
    public void Editor_HideView()
    {
        animIn.Editor_Init();
        animOut.Editor_Init();
        SetTweens();
    }
    
    public DOTweenPlayerSO Editor_AnimIn()
    {
        return animIn;
    }
    
    public DOTweenPlayerSO Editor_AnimOut()
    {
        return animOut;
    }
}
#endif