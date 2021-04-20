using UnityEngine;

[CreateAssetMenu(menuName = "BlockBehaviour/Default", fileName = "DefaultBlockBehaviour")]
public class DefaultBlockBehaviour : ScriptableObject, IBlockBehaviour
{
    [SerializeField] private BlockBehaviourMixin baseBehaviour;
    
    private Block currentBlock;
    private Block lastValidBlock;

    public bool IsValidSelection(Block selectedBlock)
    {
        currentBlock = selectedBlock;
        
        if (baseBehaviour.selectedBlockSet.IsFirstSelection() || 
            (IsBlockSameType() && IsBlockNeighbors() && IsNewlySelectedBlock()))
        {
            return true;
        }

        return false;
        
        #region Local Functions
        
        bool IsBlockSameType()
        {
            return baseBehaviour.selectedBlockSet.CoordList.Count > 0 && selectedBlock.Type == lastValidBlock.Type;
        }

        bool IsBlockNeighbors()
        {
            return baseBehaviour.neighborGrid.CoordinateHashSet[lastValidBlock.Coord.X, lastValidBlock.Coord.Y]
                .Contains(selectedBlock.Coord);
        }
    
        bool IsNewlySelectedBlock()
        {
            return baseBehaviour.selectedBlockSet.HashSet.Contains(selectedBlock) == false;
        }
        
        #endregion
    }
    
    public void SetAffectedBlocks()
    {
        lastValidBlock = currentBlock;
        lastValidBlock.Coord = currentBlock.Coord;
            
        baseBehaviour.selectedBlockSet.CoordList.Add(currentBlock.Coord);
        baseBehaviour.selectedBlockSet.HashSet.Add(currentBlock);
    }
}