using Sirenix.OdinInspector;
using UnityEngine;

public abstract class TimeMode : InitializableSO
{
    [EnumToggleButtons]
    public TimeModeType Type;
    
    [SerializeField] protected GameStateManager gameStateManager;
    [SerializeField] private int defaultTime;

#if UNITY_EDITOR
    [BoxGroup]
    [SerializeField] private bool useTestValue;
    [BoxGroup, ShowIf("useTestValue")]
    [SerializeField] private int testTime;
#endif
    
    [ReadOnly]
    public float CurrentTime;
    [ReadOnly]
    public int CurrentTimeInt;
    
    public int DefaultTime => ReturnDefaultTime();
    
    protected override void Init()
    {
        gameStateManager.OnChangeState+= OnChangeState;
    }

    private void OnChangeState(GameStateType gameStateType)
    {
        switch (gameStateType)
        {
            case GameStateType.Prepare:
                ResetTime();
                break;
        }
    }

    public abstract void OnUpdate();

    public abstract void SetCurrentTimeInt();

    protected abstract void ResetTime();

    private int ReturnDefaultTime()
    {
#if UNITY_EDITOR
        if (useTestValue)
            return testTime;
#endif
        return defaultTime;
    }
}