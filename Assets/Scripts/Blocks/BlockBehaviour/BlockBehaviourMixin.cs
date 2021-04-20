using UnityEngine;

[CreateAssetMenu(menuName = "BlockBehaviour/BaseBlock", fileName = "BaseBlockBehaviour")]
public class BlockBehaviourMixin : InitializableSO
{
    [SerializeField] public NeighborGrid neighborGrid;
    [SerializeField] public SelectedBlockSet selectedBlockSet;
    [SerializeField] public HexGrid hexGrid;
    
    [SerializeField] private SelectionStateManager selectionStateManager;
    
    public bool IsValidSelection()
    {
        if (selectedBlockSet.IsFirstSelection() == false)
            return false;   
        
        selectionStateManager.ChangeState(SelectionState.Item);
        return true;
    }
}