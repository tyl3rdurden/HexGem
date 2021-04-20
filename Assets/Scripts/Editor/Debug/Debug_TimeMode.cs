using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public sealed class Debug_TimeMode : Debug_Base
{
    [AssetsOnly]
    [SerializeField] private TimeAttackMode timeAttackMode;
    private SerializedObject serializedTimeMode;

    private const string PrefKey = "TimeMode";

    public Debug_TimeMode()
    {
        LoadReferences();
        InitAssets();
    }

    public override void SaveReferences()
    {
        EditorPrefs.SetString(PrefKey, AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(timeAttackMode)));
    }

    public override void InitAssets()
    {
        if (timeAttackMode != null)
            serializedTimeMode = new SerializedObject(timeAttackMode);
    }
    
    public override void Layout()
    {
        serializedTimeMode.Update();
        
        using (var verticalGroup = new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            EditorGUILayout.LabelField(PrefKey);
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                PropertyField();
                serializedTimeMode.ApplyModifiedProperties();
                
                if (changeScope.changed)
                    OnDataChange();
            }
        }
    }

    public override bool ValidateObject()
    {
        if (timeAttackMode == null)
        {
            Debug.LogError("timeAttackMode Null");
            return false;
        }

        return true;
    }
    
    protected override void LoadReferences()
    {
        timeAttackMode = AssetDatabase.LoadAssetAtPath<TimeAttackMode>(AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(PrefKey)));
    }

    protected override void PropertyField()
    {
        EditorGUILayout.PropertyField(serializedTimeMode.FindProperty("useTestValue"), new GUIContent("useTestValue"));
        EditorGUILayout.PropertyField(serializedTimeMode.FindProperty("testTime"), new GUIContent("testTime"));
    }

    protected override void OnDataChange()
    {
        Undo.RecordObject(timeAttackMode, "modified timeAttackMode");
        Debug.Log("modified timeAttackMode");
    }
}