using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
[InlineProperty]
public partial class DOPunchSO : ScriptableObject, ITweenData
{
    public enum PunchType
    {
        Position,
        Scale
    }
     
    [BoxGroup("Values")] 
    public float Duration;
    
    [BoxGroup("Values")]
    public Vector3 endPos;
    
    [BoxGroup("Values"), Range(0, 10)]
    public int vibrato;
    
    [BoxGroup("Values"), Range(0, 1)]
    public float elasticity;

    [BoxGroup("Values"), EnumToggleButtons] 
    public PunchType punchType;

    [BoxGroup("Values")] 
    public bool IgnoreTimeScale;

     public IProcessedTween CreateTween(ITweenProcessor tweenProcessor, Transform target)
     {
         return tweenProcessor.ProcessPunchTweenData(this, target);
     }
}

#if UNITY_EDITOR
public partial class DOPunchSO
{
    [Title("Reset")]
    [Button]
    private void ResetValuesButton()
    {
        Duration = 0;
        endPos = Vector3.zero;
        vibrato = 0;
        elasticity = 0;
    }
}
#endif

public class ProcessedDOPunchTween : IProcessedTween
{
    private DOPunchSO data;
    
    private DOGetter<Vector3> valueGetter { get; set; }
    private DOSetter<Vector3> valueSetter { get; set; }
    
    private TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> thisTween = null;

    public void CreateTween(DOPunchSO data, Transform target)
    {
        this.data = data;
        SetGetterSetter(target);
        thisTween = CreateNewTween();
    }

    public void UpdateTweenValues()
    {
        thisTween = thisTween.ChangeEndValue(GetEndValues());
        
        #region LocalFunction
        //From DOTween.cs Punch()
        Vector3[] GetEndValues()
        {
            float magnitude = data.endPos.magnitude;
            int length = (int) (data.vibrato * data.Duration);
            float num1 = magnitude / length;
            if (length < 2)
                length = 2;
                
            Vector3[] endValues = new Vector3[length];
            for (int index = 0; index < length; ++index)
            {
                if (index < length - 1)
                {
                    endValues[index] = index != 0 ? (index % 2 == 0 ? Vector3.ClampMagnitude(data.endPos, magnitude) 
                        : -Vector3.ClampMagnitude(data.endPos, magnitude * data.elasticity)) : data.endPos;
                    magnitude -= num1;
                }
                else
                    endValues[index] = Vector3.zero;
            }
        
            return endValues;
        }
        #endregion
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
        
        switch (data.punchType)
        {
            case DOPunchSO.PunchType.Position:
                valueGetter = () => rectTransform.anchoredPosition;
                valueSetter = x => rectTransform.anchoredPosition = x;
                break;
            case DOPunchSO.PunchType.Scale:
                valueGetter = () => rectTransform.localScale;
                valueSetter = x => rectTransform.localScale = x;
                break;
        }
    }
    
    private TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> CreateNewTween()
    {
        return DOTween.Punch(valueGetter, valueSetter, data.endPos, data.Duration, data.vibrato, data.elasticity).SetUpdate(data.IgnoreTimeScale);
    }
}