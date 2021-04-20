using UnityEngine;
using UnityEngine.UI;

public class GridShufflePopupController : PopupController
{
    [Header("Dependencies")]
    [SerializeField] private GridMatchValidator gridMatchValidator;
    
    [Header("UI")]
    [SerializeField] private Button okBtn;

    protected override void Init()
    {
        base.Init();
        
        transform.localPosition = Vector3.zero;
        
        okBtn.onClick.AddListener(OnClickOKBtn);
        SubscribeStateChanges();
    }

    private void SubscribeStateChanges()
    {
        gridMatchValidator.OnInvalidGrid += ShowView;
    }
    
    private void OnClickOKBtn()
    {
        gridMatchValidator.ShuffleGrid();
        HideView();
    }
}
