using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/HexGrid", fileName = "HexGrid")]
public partial class HexGrid : SerializedScriptableObject
{
    [BoxGroup, ShowInInspector]
    public const byte Width = 7;
    [BoxGroup, ShowInInspector]
    public const byte Height = 6;

    [TableMatrix(HorizontalTitle = "Width", VerticalTitle = "Height", Transpose = false,
         DrawElementMethod = "DrawBlockCell", SquareCells = true), ShowInInspector, ReadOnly, NonSerialized]
    private Block[,] Blocks = new Block[Width, Height];
    
    [TableMatrix(HorizontalTitle = "Width", VerticalTitle = "Height", Transpose = false, SquareCells = true), ShowInInspector, ReadOnly]
    public bool[,] BlocksActive = new bool[Width, Height];
    
    [TableMatrix(HorizontalTitle = "Width", VerticalTitle = "Height", Transpose = false), ShowInInspector, ReadOnly]
    public Vector3[,] BlockPositions = new Vector3[Width, Height];

    public Dictionary<GameObject, Block> GameObjectToBlockDic = new Dictionary<GameObject, Block>();

    [SerializeField] private int OffsetHeight;

    #region Blocks Get/Set
    
    public Block GetBlockAt(BlockCoord coord) => GetBlockAt(coord.X, coord.Y);
    
    public Block GetBlockAt(byte width, byte height)
    { 
        return Blocks[width, height];
    }

    public Block SetBlockAt(BlockCoord coord, Block block) => SetBlockAt(coord.X, coord.Y, block);
    
    public Block SetBlockAt(byte width, byte height, Block block)
    {
        return Blocks[width, height] = block;
    }

    #endregion

    public Vector3 OffsetBlockPosition(BlockCoord coord) => OffsetBlockPosition(coord.X, coord.Y);
    
    public Vector3 OffsetBlockPosition(byte width, byte height)
    {
        Vector3 temp = BlockPositions[width, height];
        temp.y += OffsetHeight;
        return temp;
    }
}

#if UNITY_EDITOR
public partial class HexGrid
{
    [Header("Editor Only")]
    [SerializeField] private BlockData blockData;
    
    private Block DrawBlockCell(Rect rect, Block block)
    {
        if (block == null)
            return null;
            
        if (BlocksActive[block.Coord.X, block.Coord.Y])
            UnityEditor.EditorGUI.DrawPreviewTexture(rect.Padding(1), BlockColorTexture(block.Type));
        else
            UnityEditor.EditorGUI.DrawPreviewTexture(rect.Padding(1), BlockColorTexture(BlockType.Item_Start));
        return block;
    }

    private Texture2D BlockColorTexture(BlockType type)
    {
        string path = $"Assets/Resources/Atlas/Block/{blockData.Sprite(type).name}.png";
        
        return UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }
}
#endif