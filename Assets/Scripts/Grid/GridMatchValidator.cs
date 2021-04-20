using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/GridMatchValidator", fileName = "GridMatchValidator")]
public class GridMatchValidator : InitializableSO
{
    [SerializeField] private HexGrid hexGrid;
    [SerializeField] private NeighborGrid neighborGrid;

    public Action OnInvalidGrid;
    public Action OnShuffleGrid;
    
    private readonly List<BlockCoord> sameTypeNeighborCoords = new List<BlockCoord>();

    protected override void Init()
    {
        OnInvalidGrid = null;
        OnShuffleGrid = null;
    }

    public void ValidateGrid()
    {
        for (byte width = 0; width < HexGrid.Width; width++)
        {
            for (byte height = 0; height < HexGrid.Height; height++)
            { 
                Block currentBlock = hexGrid.GetBlockAt(width, height);
                BlockCoord currentBlockCoord = currentBlock.Coord;
                BlockType currentBlockType = currentBlock.Type;

                if (IsCurrentBlockItem())
                    return;

                sameTypeNeighborCoords.Clear();
                IterateCurrentNeighborsAndFindSameType();

                if (SameTypeNeighborsMakeMatch())
                    return;

                if (SameTypeNeighborExist() && IterateSameTypeNeighborNeighborAndFindMatch())
                    return;
                
                #region Local Functions

                bool IsCurrentBlockItem()
                {
                    return currentBlockType > BlockType.Item_Start;
                }
                
                void IterateCurrentNeighborsAndFindSameType()
                {
                    BlockCoord[] currentBlockNeighbors = neighborGrid.Coordinates[currentBlockCoord.X, currentBlockCoord.Y];
                    foreach (BlockCoord neighborBlockCoord in currentBlockNeighbors)
                    {
                        if (neighborBlockCoord.IsValid 
                            && currentBlockType == hexGrid.GetBlockAt(neighborBlockCoord.X, neighborBlockCoord.Y).Type)
                        {
                            //if same type and valid, add to list/hashset
                            sameTypeNeighborCoords.Add(neighborBlockCoord);
                        }
                    }
                }

                bool SameTypeNeighborsMakeMatch()
                {
                    return sameTypeNeighborCoords.Count >= (BlockSelectionProcessor.MinMatchCount - 1);
                }
                
                bool SameTypeNeighborExist()
                {
                    return sameTypeNeighborCoords.Count > 0;
                }
                
                bool IterateSameTypeNeighborNeighborAndFindMatch()
                {
                    var sameTypeNeighborCoordsArray = sameTypeNeighborCoords.ToArray();
                    foreach (var sameTypeNeighborCoord in sameTypeNeighborCoordsArray) //only 1 if combo == 3 match but can be more if combo rule changes to combo > 3
                    {
                        BlockCoord[] sameTypeNeighborNeighborCoords = neighborGrid.Coordinates[sameTypeNeighborCoord.X, sameTypeNeighborCoord.Y];
                        
                        foreach (BlockCoord neighborNeighborBlockCoord in sameTypeNeighborNeighborCoords)
                        {
                            if (neighborNeighborBlockCoord == currentBlockCoord)
                                continue;
                        
                            if (neighborNeighborBlockCoord.IsValid 
                                && currentBlockType == hexGrid.GetBlockAt(neighborNeighborBlockCoord.X, neighborNeighborBlockCoord.Y).Type)
                            {
                                sameTypeNeighborCoords.Add(neighborNeighborBlockCoord);
                                if (SameTypeNeighborsMakeMatch())
                                    return true;
                            }
                        }
                    }

                    return false;
                }
                
                #endregion
            }
        }

        OnInvalidGrid?.Invoke();
    }

    public void ShuffleGrid()
    {
        OnShuffleGrid?.Invoke();
    }
}