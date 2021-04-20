using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "BlockManager/BlockSpawnManager", fileName =  "BlockSpawnManager")]
public partial class BlockSpawnManager : InitializableSO
{
    [SerializeField] private HexGrid hexGrid;
    
    [SerializeField] private BlockSpawner blockSpawner;
    [SerializeField] private BlockMover blockMover;

    [SerializeField] private GridMatchValidator gridMatchValidator;

    [SerializeField] private NewlySpawnedBlockSet newlySpawnedBlockSet;
    
    private bool isFirstGame = true;

    protected override void Init()
    {
        isFirstGame = true;
#if UNITY_EDITOR
        usePreGeneratedGrid = UsePreGeneratedGrid;
#endif
    }

    protected override void PostAwake()
    {
        gridMatchValidator.OnShuffleGrid += SpawnNewGrid;
    }

    public void InitBlockSpawner(Transform transform)
    {
        blockSpawner.SetBlockContainer(transform);
    }

    public void SpawnAllBlocks()
    {
        for (byte width = 0; width < HexGrid.Width; width++)
        {
            for (byte height = 0; height < HexGrid.Height; height++)
            {
                Block currentBlock = blockSpawner.SpawnBlock(width, height);
                blockMover.AnimateBlockAt(currentBlock.Coord); 
            }
        }
    }
    
    public void SpawnNewBlocks()
    {
        newlySpawnedBlockSet.NewBlockList.Clear();
        
        for (byte width = 0; width < HexGrid.Width; width++)
        {
            for (byte height = 0; height < HexGrid.Height; height++)
            {
                if (hexGrid.BlocksActive[width, height] == false)
                {
                    Block currentBlock = blockSpawner.SpawnBlock(width, height);
                    newlySpawnedBlockSet.NewBlockList.Add(currentBlock);
                }
            }
        }
    }

    public void DespawnAllBlocks()
    {
        for (byte width = 0; width < HexGrid.Width; width++)
        {
            for (byte height = 0; height < HexGrid.Height; height++)
            {
                blockSpawner.DespawnBlock(width, height);
            }
        }
    }
    
    public void DespawnSelectedBlocks(IReadOnlyList<BlockCoord> selectedBlockCoords)
    {
#if DEBUGGING
        Debug.Log($"SelectedBlock count {blockCoords.Count}");
#endif
        
        for (byte i = 0; i < selectedBlockCoords.Count; i++)
        {
            blockSpawner.DespawnBlock(selectedBlockCoords[i]);
        }
    }

    public void SpawnNewGrid()
    {
        if (isFirstGame == false)
            DespawnAllBlocks();
        
        SpawnAllBlocks();
        
#if UNITY_EDITOR
        SetGridToPreGeneratedGrid();
#endif
        
        isFirstGame = false;

        gridMatchValidator.ValidateGrid();
    }
}


#if UNITY_EDITOR
public partial class BlockSpawnManager
{
    [BoxGroup]
    [SerializeField] private bool UsePreGeneratedGrid;
    [BoxGroup, InlineEditor()]
    [SerializeField] private PreMadeGrid preMadeGrid;

    private bool usePreGeneratedGrid;

    private void SetGridToPreGeneratedGrid()
    {
        if (usePreGeneratedGrid)
        {
            for (byte width = 0; width < HexGrid.Width; width++)
            {
                for (byte height = 0; height < HexGrid.Height; height++)
                {
                    hexGrid.GetBlockAt(width, height).SetBlockType(preMadeGrid.Blocks[width, height]);
                }
            }

            usePreGeneratedGrid = false;
        }
    }
}
#endif