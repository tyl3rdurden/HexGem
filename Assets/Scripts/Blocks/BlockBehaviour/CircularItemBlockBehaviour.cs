using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BlockBehaviour/Item_Circular", fileName = "CircularItemBlockBehaviour")]
public class CircularItemBlockBehaviour : ScriptableObject, IBlockBehaviour
{
    public int CircularDepth;

    [SerializeField] private BlockBehaviourMixin baseBehaviour;
    
    private Block currentBlock;

    public bool IsValidSelection(Block selectedBlock)
    {
        currentBlock = selectedBlock;
        
        return baseBehaviour.IsValidSelection();
    }

    public void SetAffectedBlocks()
    {
        int currentDepth = 0;

        List<BlockCoord> blocksToIterateForNeighbors = new List<BlockCoord> {currentBlock.Coord};
        List<BlockCoord> validNeighborCoords = new List<BlockCoord>();
        
        while (currentDepth < CircularDepth)
        {
            foreach (BlockCoord blockCoord in blocksToIterateForNeighbors)
            {
                BlockCoord[] currentBlockNeighbors = baseBehaviour.neighborGrid.Coordinates[blockCoord.X, blockCoord.Y];

                foreach (BlockCoord neighborBlockCoord in currentBlockNeighbors)
                {
                    Block neighborBlock = baseBehaviour.hexGrid.GetBlockAt(neighborBlockCoord.X, neighborBlockCoord.Y);
                    
                    if (neighborBlockCoord.IsValid && NeighborIsNotItem() && NeighborIsNotAlreadySelected())
                    {
                        validNeighborCoords.Add(neighborBlockCoord);
                        baseBehaviour.selectedBlockSet.HashSet.Add(neighborBlock);
                    }
                    
                    #region Local Func

                    bool NeighborIsNotItem()
                    {
                        return neighborBlock.IsItem() == false;
                    }
                    
                    bool NeighborIsNotAlreadySelected()
                    {
                        return !baseBehaviour.selectedBlockSet.HashSet.Contains(neighborBlock);
                    }
        
                    #endregion
                }
            }

            blocksToIterateForNeighbors.Clear();
            blocksToIterateForNeighbors.AddRange(validNeighborCoords);
            
            baseBehaviour.selectedBlockSet.CoordList.AddRange(validNeighborCoords);
            
            validNeighborCoords.Clear();
            
            currentDepth++;
        }
        
        baseBehaviour.selectedBlockSet.CoordList.Add(currentBlock.Coord);
    }
}