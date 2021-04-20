#define DEBUGGING

using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
[InlineEditor()]
public class GameState : InitializableSO
{
    [SerializeField] private GameStateManager gameStateManager;
    
    public Action OnPreEnterState;
    public Action OnPostEnterState;
    public Action OnExitState;

    public GameStateType ThisState;
    
    public bool MoveOnToNextState;
    [ShowIf(nameof(MoveOnToNextState))]
    public GameStateType NextState;

    protected override void DeInit()
    {
        OnPreEnterState = null;
        OnPostEnterState = null;
        OnExitState = null;
    }

    public void PreEnterState()
    {
#if DEBUGGING
        Debug.Log($"PreEnterState {ThisState}");
#endif
        OnPreEnterState?.Invoke();
    }

    public void PostEnterState()
    {
#if DEBUGGING
        Debug.Log($"PostEnterState {ThisState}");
#endif
        OnPostEnterState?.Invoke();
        
        if (MoveOnToNextState)
            gameStateManager.ChangeState(NextState);
    }
    
    public void ExitState()
    {
#if DEBUGGING
        Debug.Log($"ExitState {ThisState}");
#endif
        OnExitState?.Invoke();
    }
}