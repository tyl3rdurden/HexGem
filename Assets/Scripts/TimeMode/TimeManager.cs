using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Type = TimeModeType;

public enum TimeModeType
{
    TimeAttack = 0,
    FreeMode,
}

[CreateAssetMenu(menuName = "Manager/TimeManager", fileName =  "TimeManager")]
public class TimeManager : SOManager
{
    [SerializeField] private GameStateManager gameStateManager;
    
    public event Action<float> OnTimeChange;
    public event Action<TimeMode> OnChangeTimeMode;
    
    [HideInInspector] public TimeMode CurrentTimeMode;
    [HideInInspector] public TimeMode NewTimeMode;
    
    [InlineEditor]
    [SerializeField] private TimeMode timeAttack;
    [InlineEditor]
    [SerializeField] private TimeMode freeMode;

    [EnumToggleButtons]
    [SerializeField] private TimeModeType defaultTimeMode;

    protected override void Init()
    {
        gameStateManager.GameStateDic[GameStateType.Prepare].OnPreEnterState += CheckIfTimeModeChanged;
        SetDefaultTimeMode();
    }

    private void SetDefaultTimeMode()
    {
        CurrentTimeMode = TimeModeTypeToTimeMode(defaultTimeMode);
        NewTimeMode = CurrentTimeMode;
    }
    
    public override void OnUpdate()
    {
        if (gameStateManager.currentStateType != GameStateType.InGame)
            return;
        
        CurrentTimeMode.OnUpdate();
        OnTimeChange?.Invoke(CurrentTimeMode.CurrentTime);
    }

    //on click time mode setting
    public void ChangeCurrentTimeMode(Type type)
    {
        NewTimeMode = TimeModeTypeToTimeMode(type);
        
        OnChangeTimeMode?.Invoke(NewTimeMode);
    }

    private void CheckIfTimeModeChanged()
    {
        CurrentTimeMode = NewTimeMode;
    }

    private TimeMode TimeModeTypeToTimeMode(Type type)
    {
        switch (type)
        {
            case Type.TimeAttack:
                return timeAttack;
            case Type.FreeMode:
                return freeMode;
        }

        return timeAttack;
    }
}