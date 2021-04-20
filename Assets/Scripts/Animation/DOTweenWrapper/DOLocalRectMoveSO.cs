using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public partial class DOLocalRectMoveSO : ScriptableObject, ITweenData
{
    public enum MoveType
    {
        Position,
        Scale
    }

    [BoxGroup("Values")] 
    public float Duration;
    
    [BoxGroup("Values")] 
    public Vector3 endPos;

    [BoxGroup("Values")] 
    public Ease ease = Ease.Linear;

    [BoxGroup("Values"), ShowIf("ease", Ease.INTERNAL_Custom)]
    public AnimationCurve customCurve;

    [BoxGroup("Values"), EnumToggleButtons] 
    public MoveType moveType = MoveType.Position;
    
    [BoxGroup("Values")] 
    public bool IgnoreTimeScale;

    public IProcessedTween CreateTween(ITweenProcessor tweenProcessor, Transform target)
    {
        return tweenProcessor.ProcessMoveTweenData(this, target);
    }
}

#if UNITY_EDITOR
public partial class DOLocalRectMoveSO
{
    [Title("Reset")]
    [Button]
    private void ResetValuesButton()
    {
        Duration = 0;
        endPos = Vector3.zero;
        ease = Ease.Linear;
    }
}
#endif

public class ProcessedDORectMoveTween : IProcessedTween
{
    private DOLocalRectMoveSO data;
    
    private DOGetter<Vector3> valueGetter { get; set; }
    private DOSetter<Vector3> valueSetter { get; set; }
    
    private TweenerCore<Vector3, Vector3, VectorOptions> thisTween = null;

    public void CreateTween(DOLocalRectMoveSO data, Transform target)
    {
        this.data = data;
        SetGetterSetter(target);
        thisTween = CreateNewTween();
        SetEase();
    }

    public void UpdateTweenValues()
    {
        thisTween = thisTween.ChangeValues(valueGetter(), data.endPos, data.Duration);
        SetEase();
    }
    
    public void SetToEndValue()
    {
        valueSetter(data.endPos);
    }

    public Tweener GetTween()
    {
        return thisTween;
    }

    private void SetGetterSetter(Transform transform)
    {
        RectTransform rectTransform = (RectTransform)transform;
        
        switch (data.moveType)
        {
            case DOLocalRectMoveSO.MoveType.Position:
                valueGetter = () => rectTransform.anchoredPosition;
                valueSetter = x => rectTransform.anchoredPosition = x;
                break;
            case DOLocalRectMoveSO.MoveType.Scale:
                valueGetter = () => rectTransform.localScale;
                valueSetter = x => rectTransform.localScale = x;
                break;
        }
    }
    
    private TweenerCore<Vector3, Vector3, VectorOptions> CreateNewTween()
    {
        return DOTween.To(valueGetter, valueSetter, data.endPos, data.Duration);
    }

    private void SetEase()
    {
        if (data.ease != Ease.INTERNAL_Custom)
            thisTween.SetEase(data.ease);
        else 
            thisTween.SetEase(data.customCurve);
        
        thisTween.SetUpdate(data.IgnoreTimeScale);
    }
}