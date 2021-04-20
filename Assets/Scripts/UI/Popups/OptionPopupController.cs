using UnityEngine;
using UnityEngine.UI.Extensions;

// public partial class OptionPopupController
// {
//     protected override void OnFinish()
//     {
//         ShowView();
//     }
// }

public partial class OptionPopupController : PopupController
{
    [Header("Dependencies")]
    [SerializeField] private TimeManager timeManager;
    
    [Header("UI")]
    [SerializeField] private SegmentedControl timeModeToggle;
    

    protected override void Init()
    {
        base.Init();
        timeModeToggle.onValueChanged.AddListener(OnTimeModeChanged);
    }

    protected override void OnShow()
    {
        base.OnShow();
        timeModeToggle.selectedSegmentIndex = (int)timeManager.NewTimeMode.Type;
    }

    private void OnTimeModeChanged(int type)
    {
        timeManager.ChangeCurrentTimeMode((TimeModeType)timeModeToggle.selectedSegmentIndex);
    }
}