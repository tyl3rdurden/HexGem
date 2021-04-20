using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public sealed class Debug_BlockSpawnManager : Debug_Base
{
    [AssetsOnly]
    [SerializeField] private BlockSpawnManager blockSpawnManager;
    private SerializedObject serializedBlockManager;

    private const string PrefKey = "BlockSpawnManager";

    public Debug_BlockSpawnManager()
    {
        LoadReferences();
        InitAssets();
    }

    public override void SaveReferences()
    {
        EditorPrefs.SetString(PrefKey, AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(blockSpawnManager)));
    }

    public override void InitAssets()
    {
        if (blockSpawnManager != null)
            serializedBlockManager = new SerializedObject(blockSpawnManager);
    }
    
    public override void Layout()
    {
        serializedBlockManager.Update();
        
        using (var verticalGroup = new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            EditorGUILayout.LabelField(PrefKey);
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                PropertyField();
                serializedBlockManager.ApplyModifiedProperties();
                
                if (changeScope.changed)
                    OnDataChange();
            }
        }
    }

    public override bool ValidateObject()
    {
        if (blockSpawnManager == null)
        {
            Debug.LogError("blockSpawnManager Null");
            return false;
        }

        return true;
    }
    
    protected override void LoadReferences()
    {
        blockSpawnManager = 
            AssetDatabase.LoadAssetAtPath<BlockSpawnManager>(AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(PrefKey)));
    }

    protected override void PropertyField()
    {
        EditorGUILayout.PropertyField(serializedBlockManager.FindProperty("UsePreGeneratedGrid"), new GUIContent("UsePreGeneratedGrid"));
        EditorGUILayout.PropertyField(serializedBlockManager.FindProperty("preMadeGrid"), new GUIContent("preMadeGrid"));
    }

    protected override void OnDataChange()
    {
        Undo.RecordObject(blockSpawnManager, "modified blockSpawnManager");
        Debug.Log("modified blockSpawnManager");
    }
}