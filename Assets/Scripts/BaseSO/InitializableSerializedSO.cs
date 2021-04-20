//Able to initialize SO properly in Editor thanks to:
//https://forum.unity.com/threads/solved-but-unhappy-scriptableobject-awake-never-execute.488468/#post-4483018
//and
//https://github.com/jedybg/yaSingleton

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class InitializableSerializedSO : SerializedScriptableObject
{
    private static readonly HashSet<InitializableSerializedSO> AllInitializableSO = new HashSet<InitializableSerializedSO>();
    
    protected virtual void Awake()
    {
#if !UNITY_EDITOR
        AllInitializableSO.Add(this);
#endif
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeScriptableObjects() 
    {
        foreach(var SO in AllInitializableSO) 
        {
            SO.Init();
        }
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void PostAwakeScriptableObjects() 
    {
        foreach(var SO in AllInitializableSO) 
        {
            SO.PostAwake();
        }
    }
    
    //called RuntimeInitializeLoadType.BeforeSceneLoad
    protected virtual void Init() { }    
    
    //called during RuntimeInitializeLoadType.AfterSceneLoad (Post Awake / Pre Start)
    protected virtual void PostAwake() { }

    protected virtual void DeInit() { }
    
#if UNITY_EDITOR
    private void OnEnable() 
    {
        UnityEditor.EditorApplication.playModeStateChanged += OnPlayStateChange;
        AllInitializableSO.Add(this);
    }

    private void OnDisable() 
    {
        UnityEditor.EditorApplication.playModeStateChanged -= OnPlayStateChange;
    }
   
    void OnPlayStateChange(UnityEditor.PlayModeStateChange state)
    {
        if (state == UnityEditor.PlayModeStateChange.EnteredPlayMode)
        {
            //No real usage due to only being called AFTER Mono Awake, Start etc.
        }
        else if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
        {
            DeInit();
        }
    }
#endif
}