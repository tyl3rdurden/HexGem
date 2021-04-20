using UnityEngine;

[CreateAssetMenu(menuName = "BlockManager/BlockMoveManager", fileName =  "BlockMoveManager")]
public class BlockMoveManager : InitializableSO
{
    [SerializeField] private HexGrid hexGrid;
    [SerializeField] private BlockMover blockMover;
    [SerializeField] private BlockBouncer blockBouncer;

    public void MoveBlocks()
    {
        for (byte width = 0; width < HexGrid.Width; width++)
        {
            for (byte height = 0; height < HexGrid.Height; height++)
            {
                if (hexGrid.GetBlockAt(width, height).NeedsToUpdatePosition)
                    blockMover.AnimateBlockAt(width, height);
            }
        }
    }

    public void BounceBlock(BlockCoord blockCoord)
    {
        blockBouncer.AnimateBlockAt(blockCoord);
    }

    public void ResetBouncedBlocks()
    {
        blockBouncer.ResetTweens();
    }
}