using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;

    [SerializeField] private BlockSpawnManager blockSpawnManager;
    [SerializeField] private BlockMoveManager blockMoveManager;
    
    [SerializeField] private BlockSelectionProcessor blockSelectionProcessor;
    [SerializeField] private BlockDataShifter blockDataShifter;
    [SerializeField] private ItemGenerator itemGenerator;
    [SerializeField] private GridMatchValidator gridMatchValidator;

    [SerializeField] private SelectedBlockSet selectedBlockSet;

    private List<BlockCoord> modifiedSelectedBlockCoords;

    private void Awake()
    {
        gameStateManager.OnChangeState += OnChangeState;
        blockSelectionProcessor.OnSelectBlock += OnSelectBlock;
        blockSelectionProcessor.OnMatchBlocks += OnMatchBlocks;

        blockSpawnManager.InitBlockSpawner(transform);
    }

    private void OnChangeState(GameStateType gameStateType)
    {
        switch (gameStateType)
        {
            case GameStateType.Prepare:
                OnPrepareGame();
                break;
        }
    }

    private void OnPrepareGame()
    {
        blockSpawnManager.SpawnNewGrid();
    }

    private void OnSelectBlock(BlockCoord blockCoord)
    {
        blockMoveManager.BounceBlock(blockCoord);
    }

    private void OnMatchBlocks()
    {
        blockMoveManager.ResetBouncedBlocks(); //has to be reset before despawn or else tween value will not be properly reset (blocks can respawn with smaller scale as tween was killed mid animation)
        
        modifiedSelectedBlockCoords = new List<BlockCoord>(selectedBlockSet.CoordList);
        itemGenerator.SpawnItemAt(ref modifiedSelectedBlockCoords);
        
        blockSpawnManager.DespawnSelectedBlocks(modifiedSelectedBlockCoords);
        blockDataShifter.ShiftBlocksDown();
        blockSpawnManager.SpawnNewBlocks();

        blockMoveManager.MoveBlocks();

        gridMatchValidator.ValidateGrid();
    }
}