using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "DoTweenData/DOTweenPlayerSO", fileName = "DOTweenPlayerSO")]
[InlineEditor(InlineEditorObjectFieldModes.Boxed)]
public partial class DOTweenPlayerSO : InitializableSerializedSO
{
    public event Action OnFinish;

    [ShowInInspector, ReadOnly]
    public float LongestDuration { private set; get; } //longest duration of all tweens
    
    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [SerializeField] private ITweenData[] TweenDatas;
    
    private IProcessedTween[] processedTweens;
    private Tweener longestTween;
    
    private bool IsInit = false;

    private ITweenProcessor doTweenProcessor = new DOTweenProcessor();
    
    protected override void Init()
    {
        OnFinish = null;
        LongestDuration = 0;
        processedTweens = new IProcessedTween[TweenDatas.Length];
    }
    
    public void SetTargetTransform(RectTransform transform)
    {
        IsInit = true;
        InitTween(transform);
    }
    
    public void Play()
    {   
        if (IsInit == false)
        {
#if UNITY_EDITOR
            Debug.LogError($"{this.name} tween was not initialized");
#endif
            return;
        }
        
#if UNITY_EDITOR //update tween each time we play to see updated values realtime in Editor - not needed in build
        UpdateTween();
#endif
        foreach (var tween in processedTweens)
        {
            tween.GetTween().Restart();
        }
        longestTween.OnComplete(ExecuteOnFinish);
    }

    public void ResetState()
    {
        foreach (var tween in processedTweens)
        {
            tween.GetTween().Goto(0);
        }
    }

    public void SetToEndValue()
    {
        foreach (var tween in processedTweens)
        {
            tween.SetToEndValue();
        }
    }

    public void SetDelay(float delay)
    {
        foreach (var tween in processedTweens)
        {
            tween.GetTween().SetDelay(delay);
        }
    }

    private void InitTween(RectTransform targetTransform)
    {
        for (var index = 0; index < TweenDatas.Length; index++)
        {
            processedTweens[index] = TweenDatas[index].CreateTween(doTweenProcessor, targetTransform);
            SetLongestTweenAndDuration(processedTweens[index]);
        }
    }

    private void UpdateTween()
    {
        for (var index = 0; index < processedTweens.Length; index++)
        {
            processedTweens[index].UpdateTweenValues();
            SetLongestTweenAndDuration(processedTweens[index]);
        }
    }
    
    private void SetLongestTweenAndDuration(IProcessedTween tween)
    {
        if (tween.GetTween().Duration() > LongestDuration)
        {
            LongestDuration = tween.GetTween().Duration();
            longestTween = tween.GetTween();
        }
    }
    
    private void ExecuteOnFinish()
    {
        OnFinish?.Invoke();
    }
}

#if UNITY_EDITOR
public partial class DOTweenPlayerSO
{
    public IProcessedTween[] Editor_ProcessedTweens()
    {
        return processedTweens;
    }

    public void Editor_Init()
    {
        Init();
    }
}
#endif