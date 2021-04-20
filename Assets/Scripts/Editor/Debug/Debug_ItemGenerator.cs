using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public sealed class Debug_ItemGenerator : Debug_Base
{
    [AssetsOnly]
    [SerializeField] private ItemGenerator itemGenerator;
    private SerializedObject serializedItemGenerator;

    private const string PrefKey = "ItemGenerator";

    public Debug_ItemGenerator()
    {
        LoadReferences();
        InitAssets();
    }

    public override void SaveReferences()
    {
        EditorPrefs.SetString(PrefKey, AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(itemGenerator)));
    }

    public override void InitAssets()
    {
        if (itemGenerator != null)
            serializedItemGenerator = new SerializedObject(itemGenerator);
    }
    
    public override void Layout()
    {
        serializedItemGenerator.Update();
        
        using (var verticalGroup = new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            EditorGUILayout.LabelField(PrefKey);
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                PropertyField();
                serializedItemGenerator.ApplyModifiedProperties();
                
                if (changeScope.changed)
                    OnDataChange();
            }
        }
    }

    public override bool ValidateObject()
    {
        if (itemGenerator == null)
        {
            Debug.LogError("itemGenerator Null");
            return false;
        }

        return true;
    }
    
    protected override void LoadReferences()
    {
        itemGenerator = 
            AssetDatabase.LoadAssetAtPath<ItemGenerator>(AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(PrefKey)));
    }

    protected override void PropertyField()
    {
        EditorGUILayout.PropertyField(serializedItemGenerator.FindProperty("VerticalBlockThreshold"), new GUIContent("VerticalBlockThreshold"),
            true);
        EditorGUILayout.PropertyField(serializedItemGenerator.FindProperty("DiagonalBlockThreshold"), new GUIContent("DiagonalBlockThreshold"),
            true);
        EditorGUILayout.PropertyField(serializedItemGenerator.FindProperty("CircularBlockThreshold"), new GUIContent("CircularBlockThreshold"),
            true);
    }

    protected override void OnDataChange()
    {
        Undo.RecordObject(itemGenerator, "modified itemGenerator");
        Debug.Log("modified itemGenerator");
    }
}