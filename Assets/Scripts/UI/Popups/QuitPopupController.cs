using UnityEngine;
using UnityEngine.UI;

public class QuitPopupController : PopupController
{
    [Header("UI")]
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button quitBtn;

    protected override void Init()
    {
        base.Init();
        
        transform.localPosition = Vector3.zero;
        
        cancelBtn.onClick.AddListener(OnClickCancelBtn);
        quitBtn.onClick.AddListener(OnClickQuitBtn);
    }

    private void OnClickCancelBtn()
    {
        HideView();
    }

    private void OnClickQuitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
