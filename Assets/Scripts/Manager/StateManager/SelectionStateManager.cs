using Sirenix.OdinInspector;
using UnityEngine;

public enum SelectionState
{
    Block,
    Item
}

[CreateAssetMenu(menuName = "Manager/SelectionStateManager", fileName =  "SelectionStateManager")]
public class SelectionStateManager : InitializableSO
{
    [EnumToggleButtons] 
    [SerializeField] private SelectionState defaultState = SelectionState.Block;
    
    [ShowInInspector]
    public SelectionState CurrentState { private set; get; }

    protected override void Init()
    {
        CurrentState = defaultState;
    }

    public void ChangeState(SelectionState newGameState)
    {
        CurrentState = newGameState;
    }
}