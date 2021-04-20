using UnityEngine;

public class ApplicationPauseManager : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private PopupManager popupManager;
    
#if !UNITY_EDITOR
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && gameStateManager.currentStateType == GameStateType.InGame)
            popupManager.ShowUI<PausePopupController>();
    }
#endif
}