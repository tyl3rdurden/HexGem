using DG.Tweening;
using UnityEngine;

public interface ITweenData
{
    IProcessedTween CreateTween(ITweenProcessor tweenProcessor, Transform target);
}

public interface IProcessedTween
{
    void UpdateTweenValues();

    void SetToEndValue();

    Tweener GetTween();
}

public interface ITweenProcessor
{
    IProcessedTween ProcessMoveTweenData(DOLocalRectMoveSO moveTweenData, Transform target);
    
    IProcessedTween ProcessPunchTweenData(DOPunchSO punchTweenData, Transform target);

    IProcessedTween ProcessColorTweenData(DOLabelColorSO colorTween, Transform target);
}

public class DOTweenProcessor : ITweenProcessor
{
    public IProcessedTween ProcessMoveTweenData(DOLocalRectMoveSO moveTweenData, Transform target)
    {
        ProcessedDORectMoveTween newRectMoveTween = new ProcessedDORectMoveTween();
        newRectMoveTween.CreateTween(moveTweenData, target);
        
        return newRectMoveTween;
    }

    public IProcessedTween ProcessPunchTweenData(DOPunchSO punchTweenData, Transform target)
    {
        ProcessedDOPunchTween newPunchTween = new ProcessedDOPunchTween();
        newPunchTween.CreateTween(punchTweenData, target);
        
        return newPunchTween;
    }

    public IProcessedTween ProcessColorTweenData(DOLabelColorSO colorTween, Transform target)
    {
        ProcessedColorTween newColorTween = new ProcessedColorTween();
        newColorTween.CreateTween(colorTween, target);
        
        return newColorTween;
    }
}