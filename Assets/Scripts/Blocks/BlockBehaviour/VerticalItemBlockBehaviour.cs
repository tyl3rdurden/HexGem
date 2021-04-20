using UnityEngine;

[CreateAssetMenu(menuName = "BlockBehaviour/Item_Vertical", fileName = "VerticalItemBlockBehaviour")]
public class VerticalItemBlockBehaviour : ScriptableObject, IBlockBehaviour
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
        for (byte i = 0; i < HexGrid.Height; i++)
        {
            if (SameVerticalBlock(i).IsItem() && IsNotSelectedBlock())
                continue;
            
            baseBehaviour.selectedBlockSet.CoordList.Add(SameVerticalBlock(i).Coord);

            #region Local Func
            
            Block SameVerticalBlock(byte y)
            {
                return baseBehaviour.hexGrid.GetBlockAt(currentBlock.Coord.X, y);
            }
            
            bool IsNotSelectedBlock()
            {
                return i != currentBlock.Coord.Y;
            }
            
            #endregion
        }
    }
}