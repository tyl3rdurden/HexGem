using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/PopupManager", fileName = "PopupManager")]
public class PopupManager : InitializableSerializedSO
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private Dictionary<System.Type, UIController> popupInstances;
    
    private Stack<PopupController> activePopups = new Stack<PopupController>();

    private bool closeMenuLoop;

    protected override void Init()
    {
        popupInstances = new Dictionary<System.Type, UIController>();
        
        activePopups.Clear();
        
        closeMenuLoop = false;

        inputManager.OnBackEvent += OnBackPress;

        var gos = GameObject.FindGameObjectsWithTag("Popup");
        foreach (var VARIABLE in gos)
        {
            var popupController = VARIABLE.GetComponent<UIController>();
            
            if (popupController == null)
                continue;
            
            AddToInstance(popupController);
        }
    }

    public void AddToInstance(UIController controller)
    {
        if (popupInstances.ContainsKey(controller.GetType()) == false)
            popupInstances.Add(controller.GetType(), controller);
    }

    public void RemoveAsInstance(UIController controller)
    {
        popupInstances.Remove(controller.GetType());
    }

    public void ShowUI<T>() where T : UIController
    {
        popupInstances[typeof(T)].ShowView();
    }
    
    public void HideUI<T>() where T : UIController
    {
        popupInstances[typeof(T)].HideView();
    }
    
    public void AddToActiveUI(UIController controller)
    {
        if (controller is PopupController popupController)
        {
            activePopups.Push(popupController);
        }
    }

    public void RemoveFromActiveUI(UIController controller)
    {
        if (controller is PopupController)
        {
            activePopups.Pop();
        }
    }
    
    public void PauseGame() => pauseManager.OnPauseGame(true);
    
    public void ResumeGame() =>  pauseManager.OnPauseGame(false);

    private void OnBackPress()
    {
        if (CloseLatestUI() == false)
        {
            if (gameStateManager.currentStateType == GameStateType.InGame)
            {
                ShowUI<QuitPopupController>();
            }
        }
    }

    private bool CloseLatestUI()
    {
        if (activePopups.Count > 0)
        {
            activePopups.Peek().HideView();
            return true;
        }

        return false;
    }
}

