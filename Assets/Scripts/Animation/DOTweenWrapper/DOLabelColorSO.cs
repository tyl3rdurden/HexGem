using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[CreateAssetMenu]
[InlineProperty]
public partial class DOLabelColorSO : ScriptableObject, ITweenData
{
    [BoxGroup("Values")] 
    public float Duration;
    
    [BoxGroup("Values")]
    public Color endColor;
    
    [BoxGroup("Values")]
    public Ease ease = Ease.Linear;
    
    [BoxGroup("Values"), ShowIf("ease", Ease.INTERNAL_Custom)]
    public AnimationCurve customCurve;

    [BoxGroup("Values")] 
    public bool onlyAlpha;
    
    [BoxGroup("Values")] 
    public bool IgnoreTimeScale;
    
    public IProcessedTween CreateTween(ITweenProcessor tweenProcessor, Transform target)
    {
        return tweenProcessor.ProcessColorTweenData(this, target);
    }
}

public class ProcessedColorTween : IProcessedTween
{
    private DOLabelColorSO data;
    
    private DOGetter<Color> valueGetter { get; set; }
    private DOSetter<Color> valueSetter { get; set; }
    
    private TweenerCore<Color, Color, ColorOptions> thisTween = null;

    private TextMeshProUGUI targetLabel;

    public void CreateTween(DOLabelColorSO data, Transform target)
    {
        this.data = data;
        SetGetterSetter(target);
        thisTween = CreateNewTween();
        SetEase();
    }

    public void UpdateTweenValues()
    {
        SetEndColorForOnlyAlpha();
        thisTween = thisTween.ChangeValues(valueGetter(), data.endColor, data.Duration);
        SetEase();
    }
    
    public void SetToEndValue()
    {
        SetEndColorForOnlyAlpha();
        valueSetter(data.endColor);
    }
    
    public Tweener GetTween()
    {
        return thisTween;
    }
    
    private void SetGetterSetter(Transform transform)
    {
        targetLabel = transform.GetComponent<TextMeshProUGUI>();
        
        valueGetter = () => targetLabel.color;
        valueSetter = x => targetLabel.color = x;
    }
    
    private TweenerCore<Color, Color, ColorOptions> CreateNewTween()
    {
        SetEndColorForOnlyAlpha();
        return DOTween.To(valueGetter, valueSetter, data.endColor, data.Duration);
    }

    private void SetEase()
    {
        if (data.ease != Ease.INTERNAL_Custom)
            thisTween.SetEase(data.ease);
        else
            thisTween.SetEase(data.customCurve);
        
        thisTween.SetUpdate(data.IgnoreTimeScale);
    }
    
    private void SetEndColorForOnlyAlpha()
    {
        if (data.onlyAlpha == false)
            return;

        data.endColor.r = targetLabel.color.r;
        data.endColor.g = targetLabel.color.g;
        data.endColor.b = targetLabel.color.b;
    }
}