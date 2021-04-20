public interface IBlockBehaviour
{
    bool IsValidSelection(Block selectedBlock);
    void SetAffectedBlocks();
}