using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "BlockManager/BlockSelectionProcessor", fileName = "BlockSelectionProcessor")]
public class BlockSelectionProcessor : InitializableSO
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private HexGrid hexGrid;

    [SerializeField] private SelectionStateManager selectionStateManager;
    [SerializeField] private SelectedBlockSet selectedBlockSet;

    [ShowInInspector]
    public const int MinMatchCount = 3;
    
    public event Action<BlockCoord> OnSelectBlock;
    public event Action OnMatchBlocks;
    public event Action OnReleaseBlocks;

    protected override void Init()
    {
        SubscribeStateChanges();
    }

    private void SubscribeStateChanges()
    {
        gameStateManager.GameStateDic[GameStateType.Prepare].OnPreEnterState += ClearSelection;
        gameStateManager.GameStateDic[GameStateType.Bonus].OnPreEnterState += ClearSelection;
    }

    protected override void DeInit()
    {
        OnSelectBlock = null;
        OnMatchBlocks = null;
        OnReleaseBlocks = null;
    }

    public void OnSelect(GameObject selectedBlockGO)
    {
        Block selectedBlock = hexGrid.GameObjectToBlockDic[selectedBlockGO];

        if (selectedBlock.IsValidSelection())
        {
            selectedBlock.SetAffectedBlocks();
            OnSelectBlock?.Invoke(selectedBlock.Coord);
        }
    }

    public void OnRelease()
    {
        CheckSelection();
        ClearSelection();
    }

    private void CheckSelection()
    {
        if (IsMatchValid())
            OnMatchBlocks?.Invoke();
        else
            OnReleaseBlocks?.Invoke();
        
        selectionStateManager.ChangeState(SelectionState.Block);
        
        #region Local Functions

        bool IsMatchValid()
        {
            return selectedBlockSet.CoordList.Count >= MinMatchCount
                   || selectionStateManager.CurrentState == SelectionState.Item;
        }
        
        #endregion
    }
    
    private void ClearSelection()
    {
        selectedBlockSet.Clear();
    }
}