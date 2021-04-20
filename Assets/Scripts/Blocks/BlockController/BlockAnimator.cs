using DG.Tweening;
using UnityEngine;

public abstract class BlockAnimator : InitializableSerializedSO
{
    [SerializeField] protected HexGrid hexGrid;
    [SerializeField] protected GameStateManager gameStateManager;
    
    public void AnimateBlockAt(BlockCoord coord) =>  AnimateBlockAt(coord.X, coord.Y);

    public abstract void AnimateBlockAt(byte width, byte height);

    protected abstract Tweener SetUpTween(byte width, byte height);
}