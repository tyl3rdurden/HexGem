using UnityEngine;
using UnityEngine.UI;

public abstract class PopupController : UIController
{
    [SerializeField] protected PopupManager popupManager;
    [SerializeField] protected Button exitButton;

    [SerializeField] protected bool isBlockedFromBackButton;

    protected override void Init()
    {
        base.Init();
        
        if (exitButton != null) 
            exitButton.onClick.AddListener(HideView);
        
        popupManager.AddToInstance(this);
    }

    public sealed override void ShowView()
    {
        popupManager.AddToActiveUI(this);
        base.ShowView();
    }

    protected override void AfterMoveIn()
    {
        popupManager.PauseGame();
    }

    public sealed override void HideView()
    {
        if (isBlockedFromBackButton)
            return;
        
        popupManager.RemoveFromActiveUI(this);
        popupManager.ResumeGame();
        base.HideView();
    }
}
