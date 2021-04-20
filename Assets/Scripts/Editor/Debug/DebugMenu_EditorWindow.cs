using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEditor;

//TODO
//EditorPrefs Keys
//Make Each Serialized Object into different classes / interface it

public class DebugMenu_EditorWindow : OdinEditorWindow
{
    private static DebugMenu_EditorWindow DebugMenuEditorWindow;

    [UsedImplicitly]
    [SerializeField] private bool debugThisMenu = false;

    [HideIf("@debugThisMenu == false"), FoldoutGroup("Debug")]
    [SerializeField] private Debug_Base debug_gameStateManager;
    
    [HideIf("@debugThisMenu == false"), FoldoutGroup("Debug")]
    [SerializeField] private Debug_Base debug_timeMode;
    
    [HideIf("@debugThisMenu == false"), FoldoutGroup("Debug")]
    [SerializeField] private Debug_Base debug_countdownAnimation;
    
    [HideIf("@debugThisMenu == false"), FoldoutGroup("Debug")]
    [SerializeField] private Debug_Base debug_itemGenerator;
    
    [HideIf("@debugThisMenu == false"), FoldoutGroup("Debug")]
    [SerializeField] private Debug_Base debug_blockManager;
    
    private Debug_Base[] debugBases;

    private Vector2 _scrollPos;

    [MenuItem("DebugMenu/Window")]
    public static void Init()
    {
        DebugMenuEditorWindow = GetWindow<DebugMenu_EditorWindow>(false, "DebugMenu_EditorWindow");
        DebugMenuEditorWindow.LoadReferences();
        DebugMenuEditorWindow.InitAssets();
        DebugMenuEditorWindow.Show();
    }

    [HideIf("@debugThisMenu == false"), Button("Save Reference")]
    void SaveReferences()
    {
        foreach (var debugMenu in debugBases)
        {
            debugMenu.SaveReferences();
        }
    }
    
    void LoadReferences()
    {
        if (debug_timeMode == null)
            debug_timeMode = new Debug_TimeMode();
        
        if (debug_countdownAnimation == null)
            debug_countdownAnimation = new Debug_CountdownAnimation();
        
        if (debug_itemGenerator == null)
            debug_itemGenerator = new Debug_ItemGenerator();

        if (debug_blockManager == null)
            debug_blockManager = new Debug_BlockSpawnManager();
        
        if (debug_gameStateManager == null)
            debug_gameStateManager = new Debug_GameStateManager();

        debugBases = new[] {debug_gameStateManager, debug_timeMode, debug_countdownAnimation, debug_itemGenerator, debug_blockManager};
    }

    void InitAssets()
    {
        if (debugBases == null)
            LoadReferences();
        
        foreach (var debugMenu in debugBases)
        {
            debugMenu.InitAssets();
        }
    }

    protected override void OnGUI()
    {
        base.OnGUI();
        
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        if (debugBases == null)
            LoadReferences();
        
        foreach (var debugMenu in debugBases)
        {
            debugMenu.Layout();
        }

        EditorGUILayout.EndScrollView();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InitAssets();
    }

    private void OnFocus()
    {
        InitAssets();
    }

    private void OnSelectionChange()
    {
        InitAssets();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SaveReferences();
    }
}