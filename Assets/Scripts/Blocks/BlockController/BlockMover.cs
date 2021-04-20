//#define DEBUGGING

using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[CreateAssetMenu(menuName = "Animator/BlockMover", fileName = "BlockAnimator")]
public class BlockMover : BlockAnimator
{
    [SerializeField]
    private Dictionary<GameStateType, BlockDOTweenData> blockDoTweenDatas = new Dictionary<GameStateType, BlockDOTweenData>();

    private BlockDOTweenData currentTweenData;

    private TweenerCore<Vector3, Vector3, VectorOptions>[,] moveTweens;

    protected override void Init()
    {
        //new in init to prevent previously serialized data to be used
        moveTweens = new TweenerCore<Vector3, Vector3, VectorOptions>[HexGrid.Width, HexGrid.Height];
    }

    public override void AnimateBlockAt(byte width, byte height)
    {
        hexGrid.GetBlockAt(width, height).UpdatedPosition();

        SetCurrentTweenData();
        SetUpTween(width, height).Restart(currentTweenData.Delay(height) != 0, 
                                        currentTweenData.Delay(height));
    }

    private void SetCurrentTweenData()
    {
        if (gameStateManager.currentStateType == GameStateType.Prepare)
            currentTweenData = blockDoTweenDatas[gameStateManager.currentStateType];
        else 
            currentTweenData = blockDoTweenDatas[GameStateType.InGame];
    }

    protected override Tweener SetUpTween(byte width, byte height)
    {
        if (moveTweens[width, height] == null) //new tween
        {
            moveTweens[width, height] =
                DOTween.To(() => hexGrid.GetBlockAt(width, height).CachedTransform().localPosition,
                                x => hexGrid.GetBlockAt(width, height).CachedTransform().localPosition = x,
                                hexGrid.BlockPositions[width, height], 
                                currentTweenData.Duration());
        }
        else //modify cached tween - no GC method
        {
            moveTweens[width, height] = moveTweens[width, height].ChangeValues(
                hexGrid.GetBlockAt(width, height).CachedTransform().localPosition,
                hexGrid.BlockPositions[width, height], 
                currentTweenData.Duration());
        }

        //moveTweens[width, height].SetUpdate(true); - ignore TimeScale

        if (currentTweenData.Ease == Ease.INTERNAL_Custom)
            moveTweens[width, height].SetEase(currentTweenData.CustomCurve);
        else
            moveTweens[width, height].SetEase(currentTweenData.Ease);

#if DEBUGGING
            Debug.Log($"width {width} height {height}");
            Debug.Log($"StartPos {hexGrid.Blocks[width, height].CachedTransform().localPosition} " +
                      $"EndPos {hexGrid.BlockPositions[width, height]}");
#endif

        return moveTweens[width, height];
    }
}