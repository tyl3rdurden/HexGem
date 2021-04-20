using UnityEngine;

[CreateAssetMenu(menuName = "BlockBehaviour/Item_Diagonal", fileName = "DiagonalItemBlockBehaviour")]
public class DiagonalItemBlockBehaviour : ScriptableObject, IBlockBehaviour
{
    [SerializeField] private BlockBehaviourMixin baseBehaviour;
    
    private Block currentBlock;

    public bool IsValidSelection(Block selectedBlock)
    {
        currentBlock = selectedBlock;
        
        return baseBehaviour.IsValidSelection();
    }
    
    public void SetAffectedBlocks()
    {
        var currentBlockNeighbors = baseBehaviour.neighborGrid.Coordinates[currentBlock.Coord.X, currentBlock.Coord.Y];
        
        BlockCoord topLeft = currentBlockNeighbors[(byte) BlockDirection.TopLeft];
        BlockCoord topRight = currentBlockNeighbors[(byte) BlockDirection.TopRight];
        BlockCoord bottomLeft = currentBlockNeighbors[(byte) BlockDirection.BottomLeft];
        BlockCoord bottomRight = currentBlockNeighbors[(byte) BlockDirection.BottomRight];
        
        while (topLeft.IsValid)
        {
            if (baseBehaviour.hexGrid.GetBlockAt(topLeft).IsItem() == false)
                baseBehaviour.selectedBlockSet.CoordList.Add(topLeft);
            
            topLeft = baseBehaviour.neighborGrid.Coordinates[topLeft.X, topLeft.Y][(byte) BlockDirection.TopLeft];
        }
        
        while (topRight.IsValid)
        {
            if (baseBehaviour.hexGrid.GetBlockAt(topRight).IsItem() == false)
                baseBehaviour.selectedBlockSet.CoordList.Add(topRight);
            
            topRight = baseBehaviour.neighborGrid.Coordinates[topRight.X, topRight.Y][(byte) BlockDirection.TopRight];
        }
        
        while (bottomLeft.IsValid)
        {
            if (baseBehaviour.hexGrid.GetBlockAt(bottomLeft).IsItem() == false)
                baseBehaviour.selectedBlockSet.CoordList.Add(bottomLeft);

            bottomLeft = baseBehaviour.neighborGrid.Coordinates[bottomLeft.X, bottomLeft.Y][(byte) BlockDirection.BottomLeft];
        }
        
        while (bottomRight.IsValid)
        {
            if (baseBehaviour.hexGrid.GetBlockAt(bottomRight).IsItem() == false)
                baseBehaviour.selectedBlockSet.CoordList.Add(bottomRight);

            bottomRight = baseBehaviour.neighborGrid.Coordinates[bottomRight.X, bottomRight.Y][(byte) BlockDirection.BottomRight];
        }
        
        baseBehaviour.selectedBlockSet.CoordList.Add(currentBlock.Coord);
    }
}