using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum BlockDirection
{
    Top = 0,
    TopLeft,
    TopRight,
    Bottom,
    BottomLeft,
    BottomRight,
    END
}

[CreateAssetMenu(menuName = "Data/NeighborGrid", fileName = "NeighborGrid")]
public class NeighborGrid : SerializedScriptableObject
{
    [HideInInspector]
    public BlockCoord[,][] Coordinates = new BlockCoord[HexGrid.Width, HexGrid.Height][]; //for actual neighbor coordinates
    
    [HideInInspector]
    public HashSet<BlockCoord>[,] CoordinateHashSet = new HashSet<BlockCoord>[HexGrid.Width, HexGrid.Height]; //for simple check of whether coord is a neighbor

#if UNITY_EDITOR
    [Button(ButtonSizes.Large)]
    private void CalculateNeighbors()
    {
        for (byte width = 0; width < HexGrid.Width; width++)
        {
            for (byte height = 0; height < HexGrid.Height; height++)
            {
                CalculateNeighborBlocks(width, height);
            }
        }

        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
    }

    private void CalculateNeighborBlocks(byte width, byte height)
    {
        HashSet<BlockCoord> currentHashSet = new HashSet<BlockCoord>();
        
        bool isAboveBlockExist = height - 1 >= 0;
        bool isBelowBlockExist = height + 1 < HexGrid.Height;

        bool isLeftBlockExist = width - 1 >= 0;
        bool isRightBlockExist = width + 1 < HexGrid.Width;
        
        BlockCoord top = TopBlock();
        BlockCoord topLeft = TopLeftBlock();
        BlockCoord topRight = TopRightBlock();
        BlockCoord bottom = BottomBlock();
        BlockCoord bottomLeft = BottomLeftBlock();
        BlockCoord bottomRight = BottomRightBlock();

        BlockCoord[] neighbors = {top, topLeft, topRight, bottom, bottomLeft, bottomRight};
        
        for (byte i = 0; i < (byte) BlockDirection.END; i++)
        {
            currentHashSet.Add(neighbors[i]);
        }

        Coordinates[width, height] = neighbors;
        CoordinateHashSet[width, height] = currentHashSet;

        BlockCoord TopBlock()
        {
            if (isAboveBlockExist)
                return new BlockCoord(width, (byte) (height - 1));

            return new BlockCoord(false);
        }
        
        BlockCoord TopLeftBlock()
        {
            if (isLeftBlockExist == false)
                return new BlockCoord(false);
            
            if (width.IsOdd() && isAboveBlockExist)
                return new BlockCoord((byte)(width - 1), (byte)(height - 1));
                
            if (width.IsEven())
                return new BlockCoord((byte) (width - 1), height);
            
            return new BlockCoord(false);
        }
        
        BlockCoord TopRightBlock()
        {
            if (isRightBlockExist == false)
                return new BlockCoord(false);
            
            if (width.IsOdd() && isAboveBlockExist)
                return new BlockCoord((byte)(width + 1), (byte)(height - 1));
            
            if (width.IsEven())
                return new BlockCoord((byte)(width + 1), height);
            
            return new BlockCoord(false);
        }
        
        BlockCoord BottomBlock()
        {
            if (isBelowBlockExist)
                return new BlockCoord(width, (byte)(height + 1));
            
            return new BlockCoord(false);
        }
        
        BlockCoord BottomLeftBlock()
        {
            if (isLeftBlockExist == false)
                return new BlockCoord(false);
            
            if (width.IsEven() && isBelowBlockExist)
                return new BlockCoord((byte)(width - 1), (byte)(height + 1));
            
            if (width.IsOdd())
                return new BlockCoord((byte)(width - 1), height);
            
            return new BlockCoord(false);
        }
        
        BlockCoord BottomRightBlock()
        {
            if (isRightBlockExist == false)
                return new BlockCoord(false);
            
            if (width.IsEven() && isBelowBlockExist)
                return new BlockCoord((byte) (width + 1), (byte) (height + 1));
            
            if (width.IsOdd())
                return new BlockCoord((byte)(width + 1), height);
            
            return new BlockCoord(false);
        }
    }
#endif
}