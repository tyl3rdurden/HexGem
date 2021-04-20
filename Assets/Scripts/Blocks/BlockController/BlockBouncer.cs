//#define DEBUGGING

using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Animator/BlockBouncer", fileName = "BlockBouncer")]
public class BlockBouncer : BlockAnimator
{
    [SerializeField] private BlockDOTweenData bounceData;

    [SerializeField] private Vector3 startScale;
    [SerializeField] private Vector3 endScale;

    private Stack<Tweener> tweenStack = new Stack<Tweener>(30);
    
    public override void AnimateBlockAt(byte width, byte height)
    {
        hexGrid.GetBlockAt(width, height).CachedTransform().localScale = startScale;
        SetUpTween(width, height).Restart(bounceData.Delay(height) != 0, 
            bounceData.Delay(height));
    }
    
    public void ResetTweens()
    {
        while (tweenStack.Count > 0)
        {
            tweenStack.Pop().Kill(true);
        }
    }

    protected override Tweener SetUpTween(byte width, byte height)
    {
        Tweener tween = DOTween.To(() => hexGrid.GetBlockAt(width, height).CachedTransform().localScale,
            x => hexGrid.GetBlockAt(width, height).CachedTransform().localScale = x,
            endScale,
            bounceData.Duration());
        
        if (bounceData.Ease == Ease.INTERNAL_Custom)
            tween.SetEase(bounceData.CustomCurve);
        else
            tween.SetEase(bounceData.Ease);

        tween.SetAutoKill(true);
        
        tweenStack.Push(tween);
        
#if DEBUGGING
            Debug.Log($"width {width} height {height}");
#endif

        return tween;
    }
}