using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : PopupController
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button creditBtn;
    [SerializeField] private Button quitBtn;

    protected override void Init()
    {
        startBtn.onClick.AddListener(OnClickStartBtn);
        quitBtn.onClick.AddListener(OnClickQuitBtn);
    }
    
    protected override void AddListeners()
    {
        //ReferenceManager.OpenMainMenuPopup += ShowView;
    }

    protected override void RemoveListeners()
    {
        //ReferenceManager.OpenMainMenuPopup -= ShowView;
    }
    
    private void OnClickStartBtn()
    {
        HideView();
        //HGGameManager.Instance.StartGame();
    }
    
    private void OnClickQuitBtn()
    {
        Application.Quit();
    }
}