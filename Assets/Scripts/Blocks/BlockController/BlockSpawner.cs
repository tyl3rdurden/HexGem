//#define DEBUGGING

using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "BlockManager/BlockSpawner", fileName =  "BlockSpawner")]
public class BlockSpawner : InitializableSO
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private HexGrid hexGrid;
    
    [InlineEditor] 
    [SerializeField] private SimplePool blockPool;

    private Transform blockContainerTransform;

    protected override void Init()
    {
        gameStateManager.OnChangeState += OnChangeState;
    }

    protected override void DeInit()
    {
        gameStateManager.OnChangeState -= OnChangeState;
    }

    private void OnChangeState(GameStateType gameStateType)
    {
        switch (gameStateType)
        {
            case GameStateType.Load:
                OnLoad();
                break;
        }
    }

    private void OnLoad()
    {
        blockPool.Preload(70);
    }
    
    public void SetBlockContainer(Transform transform)
    {
        blockContainerTransform = transform;
    }

    public Block SpawnBlock(BlockCoord blockCoord) => SpawnBlock(blockCoord.X, blockCoord.Y);
    
    public Block SpawnBlock(byte width, byte height)
    {
        Block block = blockPool.Spawn(hexGrid.OffsetBlockPosition(width, height), true, blockContainerTransform);
        block.Coord = new BlockCoord(width, height);
        block.ChangeName(width, height);
        block.SetBlockType(RandomBlockType());

        hexGrid.BlocksActive[width, height] = true;
        return hexGrid.SetBlockAt(width, height, block);
    }

    public void DespawnBlock(BlockCoord blockCoord) => DespawnBlock(blockCoord.X, blockCoord.Y);
    
    public void DespawnBlock(byte width, byte height)
    {
        hexGrid.BlocksActive[width, height] = false;
        blockPool.Despawn(hexGrid.GetBlockAt(width, height));
        
#if DEBUGGING        
        Debug.Log($"Block W {blockCoord.X} H {blockCoord.Y} off");
#endif
    }

    private BlockType RandomBlockType()
    {
        return (BlockType) Random.Range(0, (int) BlockType.Item_Start);
    }
}