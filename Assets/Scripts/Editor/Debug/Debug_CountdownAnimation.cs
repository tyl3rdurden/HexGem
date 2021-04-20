using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public sealed class Debug_CountdownAnimation : Debug_Base
{
    [AssetsOnly]
    [SerializeField] private UI_InGame_CountdownAnimation countdownAnimation;
    private SerializedObject serializedCountdownAnimation;

    private const string PrefKey = "CountdownAnimation";

    public Debug_CountdownAnimation()
    {
        LoadReferences();
        InitAssets();
    }

    public override void SaveReferences()
    {
        EditorPrefs.SetString(PrefKey, AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(countdownAnimation)));
    }

    public override void InitAssets()
    {
        if (countdownAnimation != null)
            serializedCountdownAnimation = new SerializedObject(countdownAnimation);
    }
    
    public override void Layout()
    {
        serializedCountdownAnimation.Update();
        
        using (var verticalGroup = new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            EditorGUILayout.LabelField(PrefKey);
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                PropertyField();
                serializedCountdownAnimation.ApplyModifiedProperties();
                
                if (changeScope.changed)
                    OnDataChange();
            }
        }
    }

    public override bool ValidateObject()
    {
        if (countdownAnimation == null)
        {
            Debug.LogError("countdownAnimation Null");
            return false;
        }

        return true;
    }
    
    protected override void LoadReferences()
    {
        countdownAnimation = 
            AssetDatabase.LoadAssetAtPath<UI_InGame_CountdownAnimation>(AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(PrefKey)));
    }

    protected override void PropertyField()
    {
        EditorGUILayout.PropertyField(serializedCountdownAnimation.FindProperty("skipCountdown"), new GUIContent("skipCountdown"));
    }

    protected override void OnDataChange()
    {
        Undo.RecordObject(countdownAnimation, "modified countdownAnimation");
        PrefabUtility.SavePrefabAsset(countdownAnimation.gameObject);

        Debug.Log("modified countdownAnimation");
    }
}