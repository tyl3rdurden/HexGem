using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class ForegroundFXAnimator : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private FXGrid fxGrid;
    [SerializeField] private BlockSelectionProcessor blockSelectionProcessor;
    
    private TweenerCore<Vector3, Vector3, VectorOptions>[,] tweeners = new TweenerCore<Vector3, Vector3, VectorOptions>[HexGrid.Width, HexGrid.Height];

    private Stack<BlockCoord> selectedBlockCoords = new Stack<BlockCoord>(HexGrid.Width * HexGrid.Height);
    //not using SelectedBlockSet due to it being cleared on Finish
    //- ForegroundFXAnimator still needs the data to clear selected blocks
    //during Finish popup
    
    private void Start()
    {
        PopulateFXGrid();
        InputSubscription();
        gameStateManager.OnChangeState += OnChangeState;
    }

    private void OnChangeState(GameStateType gameStateType)
    {
        switch (gameStateType)
        {
            case GameStateType.Finish:
                ResetForegroundFX();
                break;
        }
    }

    private void PopulateFXGrid()
    {
        Transform cachedTransform = transform;

        int width = 0;
        int height = 0;
        
        for (int i = 0; i < cachedTransform.childCount; i++)
        {
            var child = cachedTransform.GetChild(i);
            
            if (height > HexGrid.Height - 1)
            {
                height = 0;
                width++;
            }

            fxGrid.ForegroundFXs[width, height] = (RectTransform)child;
            
            height++;
        }
    }

    private void InputSubscription()
    {
        blockSelectionProcessor.OnSelectBlock += AnimateForegroundFX;
        blockSelectionProcessor.OnReleaseBlocks += ResetForegroundFX;
        blockSelectionProcessor.OnMatchBlocks += ResetForegroundFX;
    }

    private void AnimateForegroundFX(BlockCoord blockCoord)
    {
        if (tweeners[blockCoord.X, blockCoord.Y] == null)
        {
            tweeners[blockCoord.X, blockCoord.Y] =
                DOTween.To(() => fxGrid.ForegroundFXs[blockCoord.X, blockCoord.Y].localScale,
                    x => fxGrid.ForegroundFXs[blockCoord.X, blockCoord.Y].localScale = x,
                    Vector3.one,
                    .15f);
        }
        
        tweeners[blockCoord.X, blockCoord.Y].Restart();
        
        selectedBlockCoords.Push(blockCoord);
    }

    private void ResetForegroundFX()
    {
        while (selectedBlockCoords.Count > 0)
        {
            var blockCoord = selectedBlockCoords.Pop();
            tweeners[blockCoord.X, blockCoord.Y].Kill();
            tweeners[blockCoord.X, blockCoord.Y] = null;
            
            fxGrid.ForegroundFXs[blockCoord.X, blockCoord.Y].localScale = Vector3.zero;
        }
        
        selectedBlockCoords.Clear();
    }
}
