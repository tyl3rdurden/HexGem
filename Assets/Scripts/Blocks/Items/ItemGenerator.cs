using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

//TODO: Make BlockThreshold part of a list
//Make block items a list so that ItemGenerator code
//doesnt have to be changed everytime new item is added
//- but its fine for now as no need to create seperate editor script
//to be able to see all threshold values at a glance

[CreateAssetMenu(menuName = "BlockManager/ItemGenerator", fileName =  "ItemGenerator")]
public class ItemGenerator : ScriptableObject
{
    [SerializeField] private HexGrid hexGrid;
    [SerializeField] private SelectionStateManager selectionStateManager;
    [SerializeField] private SelectedBlockSet selectedBlockSet;

    [BoxGroup]
    [SerializeField] private byte CircularBlockThreshold;
    [BoxGroup]
    [SerializeField] private byte DiagonalBlockThreshold;
    [BoxGroup]
    [SerializeField] private byte VerticalBlockThreshold;

    private int matchCount;
    
    public void SpawnItemAt(ref List<BlockCoord> selectedBlockCoords)
    {
        if (DoesItemSpawn() == false)
            return;

        int lastBlock = selectedBlockCoords.Count - 1;
        BlockCoord itemBlockCoord = selectedBlockCoords[lastBlock];

        Block block = hexGrid.GetBlockAt(itemBlockCoord);
        
        if (matchCount >= CircularBlockThreshold)
            block.SetBlockType(BlockType.Item_Circular);
        
        else if (matchCount >= DiagonalBlockThreshold)
            block.SetBlockType(BlockType.Item_Diagonal);
        
        else if (matchCount >= VerticalBlockThreshold)
            block.SetBlockType(BlockType.Item_Vertical);
        
        selectedBlockCoords.RemoveAt(lastBlock);
    }
    
    private bool DoesItemSpawn()
    {
        if (selectionStateManager.CurrentState == SelectionState.Item)
            return false;
        
        matchCount = selectedBlockSet.CoordList.Count;

        if (matchCount >= VerticalBlockThreshold)
            return true;

        return false;
    }
}