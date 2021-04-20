using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "BlockManager/ItemActivator", fileName =  "ItemActivator")]
public class ItemActivator : InitializableSO
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private HexGrid hexGrid;
    [SerializeField] private BlockSelectionProcessor blockSelectionProcessor;

    [SerializeField] private float itemActivationDelay = .9f;
    
    private HashSet<Block> activeItems = new HashSet<Block>();

    protected override void Init()
    {
        activeItems.Clear();
    }
    
    public void ActivateRemainingItems()
    {
        ActivateRemainingItemsAsync().Forget();
    }

    async UniTaskVoid ActivateRemainingItemsAsync()
    {
        FindItems();
        
        foreach (var itemBlock in activeItems)
        {
            blockSelectionProcessor.OnSelect(itemBlock.gameObject);
            blockSelectionProcessor.OnRelease();
            
            await UniTask.Delay(TimeSpan.FromSeconds(itemActivationDelay), ignoreTimeScale: false);
        }

        activeItems.Clear();

        gameStateManager.ChangeState(GameStateType.Finish);
        
        await UniTask.Yield();
    }

    private void FindItems()
    {
        for (byte width = 0; width < HexGrid.Width; width++)
        {
            for (byte height = 0; height < HexGrid.Height; height++)
            {
                Block block = hexGrid.GetBlockAt(width, height);
                
                if (block.IsItem())
                {
                    activeItems.Add(block);
                }
            }
        }
    }
}