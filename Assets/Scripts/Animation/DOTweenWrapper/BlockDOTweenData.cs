using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor]
[CreateAssetMenu(menuName = "DoTweenData/BlockDOTweenData", fileName = "BlockDOTweenData")]
public class BlockDOTweenData : ScriptableObject
{    
    public enum MoveDelayType
    {
        None,
        StartGame,
        Normal
    }

    [BoxGroup]
    [SerializeField] private float duration;

    [EnumToggleButtons] 
    [SerializeField] private MoveDelayType delayType;
    
    [BoxGroup, ShowIf("delayType", MoveDelayType.Normal)]
    [SerializeField] private float delay;
    
    [BoxGroup]
    public Ease Ease;

    [BoxGroup, ShowIf("Ease", Ease.INTERNAL_Custom)]
    public AnimationCurve CustomCurve;

    public float Delay(int height)
    {
        switch (delayType)
        {
            case MoveDelayType.None:
                return 0;
            case MoveDelayType.StartGame:
                return (HexGrid.Height - height) * .1f;
            case MoveDelayType.Normal:
                return delay;
        }

        return -1;
    }

    public float Duration()
    {
        return duration;
    }
}